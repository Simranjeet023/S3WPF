using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Win32;
using Amazon.S3.Transfer;

namespace _301096970_singh__Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IAmazonS3 s3Client;
        private string accessKeyID = "Your AccessKeyID";
        private string secrectKey = "Your SecretKey";
        private List<string> objectNames = new List<string>();
        private string filePath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Create(object sender, RoutedEventArgs e)
        {
            string bucketName = txtBucketName.Text;
            if (bucketName == "")
            {
                MessageBox.Show("Please enter bucket name");
            }
            else
            {
                try
                {
                    //creating bucket
                    PutBucketRequest request = new PutBucketRequest();
                    request.BucketName = bucketName;
                    s3Client = new AmazonS3Client(accessKeyID, secrectKey, RegionEndpoint.USEast1);

                    s3Client.PutBucket(request);
                    MessageBox.Show("Bucket created successfully");
                }
                catch (AmazonS3Exception ex)
                {
                    // If bucket or object does not exist
                    MessageBox.Show("Error while creating bucket: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while creating bucket: " + ex.Message);
                }
            }
        }

        private void Button_Browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string temp = "";
                txtObjectName.Text = System.IO.Path.GetFileName(openFileDialog.FileName);
                filePath = openFileDialog.FileName;
            }
        }

        private void Button_Upload(object sender, RoutedEventArgs e)
        {
            try
            {
                s3Client = new AmazonS3Client(accessKeyID, secrectKey, RegionEndpoint.USEast1);

                // create a TransferUtility instance passing it the IAmazonS3 created in the first step
                TransferUtility utility = new TransferUtility(s3Client);
                // making a TransferUtilityUploadRequest instance
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                //  request.Key = fileNameInS3; //file name up in S3
                request.BucketName = txtBucketName.Text; // your bucket name it should be dynamic by seleting from existing link

                request.FilePath = filePath; //local file name
                utility.Upload(request); //commensing the transfer

                MessageBox.Show("File is successfully uploaded to : " + txtBucketName.Text.ToString());

            }
            catch (AmazonS3Exception ex)
            {
                // If bucket or object does not exist
                MessageBox.Show("Error while uploading object: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while uploading object: " + ex.Message);
            }
        }
    }
}
