using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using ZIndex.DNN.OpenStoreImport.Import;
using ZIndex.DNN.OpenStoreImport.Logger;
using ZIndex.DNN.OpenStoreImport.Model.Window;
using ZIndex.DNN.OpenStoreImport.Properties;
using Application = System.Windows.Application;

namespace ZIndex.DNN.OpenStoreImport
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowEntity _entity;
        private readonly ILog _log = new LoggerBase(typeof(MainWindow)).Logger;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _entity = new MainWindowEntity
            {
                UnitCost = Settings.Default.UnitCost.ToString(CultureInfo.InvariantCulture),
                Culture = Settings.Default.Culture,
                ImageBasePath = Settings.Default.ImageBasePath,
                ImageBaseUrl = Settings.Default.ImageBaseUrl,
                GenerateZip = true,
            };
        }

        /// <summary>
        ///     Select the Src Folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSrcOpenFile_Click(object sender, RoutedEventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a folder.
            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            _entity.SrcPath = dialog.SelectedPath;
            _log.Debug("User selected SrcPath: {0}", _entity.SrcPath);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnGenerate_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                new Thread(Generate).Start();
            }
            catch (Exception ex)
            {
                AppendStatusText(@"Une erreur s'est produite lors de la génération du fichier :");
                AppendStatusText(ex.Message + "\n\r" + ex.StackTrace);
            }
        }

        private void Generate()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait);

                AppendStatusText($"--- Début de la génération à {DateTime.Now} ---------");
                AppendStatusText(_entity.ToString());


                var importManager = new ImportManager(new StoreParser(new CategoriesParser(), new ProductsParser())
//                    new ImportV4FileGenerator(new OpenStoreConverter())
                    , new WooCommerceImportFileGenerator(new StoreConverter())
                    , new ZipFileGenerator());

                importManager.GenerateImportFiles(_entity.SrcPath
                    , CultureInfo.GetCultureInfo(_entity.Culture)
                    , _entity.ImageBasePath // imageBasePath
                    , _entity.ImageBaseUrl // imageBaseUrl
                    , decimal.Parse(_entity.UnitCost) // unitCost
                    , _entity.GenerateZip
                );


                AppendStatusText($"Fichiers Xml et Zip créés dans {_entity.SrcPath}");
                AppendStatusText($"--- Fin de la génération à {DateTime.Now} ---------");
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.Message, ex);
                AppendStatusText(@"Une erreur s'est produite lors de la génération du fichier :");
                AppendStatusText(ex.Message + "\n\r" + ex.StackTrace);

            }
            finally
            {

                Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);

            }


        }



        /// <summary>
        ///     Add the text to the status panel with a carriage return by default
        /// </summary>
        /// <param name="text"></param>
        /// <param name="addCarriageReturn"></param>
        private void AppendStatusText(string text, bool addCarriageReturn = true)
        {
            Dispatcher.BeginInvoke(
                new Action(() => { TbStatus.Text += text + (addCarriageReturn ? "\n" : string.Empty); }),
                DispatcherPriority.SystemIdle
            );
        }
    }
}