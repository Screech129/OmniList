using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmniList.Helpers;
using OmniList.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace OmniList.ViewModels
{
    public class AddEditGroceryViewModel : INotifyPropertyChanged
    {
        private readonly INavigation navigation;
        private readonly DbHelper dbHelper = new DbHelper();
        public string User => InitializerHelper.Client.CurrentUser.UserId;
        public ICommand SaveGrocery { get; protected set; }
        public ICommand CancelGrocery { get; protected set; }
        public ICommand TakePicture { get; protected set; }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }

        }
        private string categoryName;
        public string CategoryName
        {
            get
            {
                return categoryName;
            }
            set
            {
                categoryName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryName)));
            }

        }

        private string analysis;
        public string Analysis
        {
            get
            {
                return analysis;
            }
            set
            {
                analysis = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Analysis)));
            }

        }

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }

        }
        private string caption;
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Caption)));
            }

        }
        private string photoText;
        public string PhotoText
        {
            get
            {
                return photoText;
            }
            set
            {
                photoText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PhotoText)));
            }

        }
        public AddEditGroceryViewModel (INavigation navigation)
        {
            this.navigation = navigation;
            SaveGrocery = new Command(async () => await SaveGroceryItem());
            CancelGrocery = new Command(async () => await CancelGroceryItem());
            TakePicture = new Command(async () => await TakePhoto());
        }

        private async Task<MediaFile> TakePhoto ()
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                {
                    Directory = "Groceries",
                    Name = $"{DateTime.UtcNow}.jpg",
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 50
                };

                var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
                AnalysePhoto(file);
                return file;
            }

            return null;
        }

        private void AnalysePhoto (MediaFile file)
        {
            try
            {
                MakeAnalysisRequest(file);
                ReadText(file);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }

        }
        static byte[] GetImageAsByteArray (MediaFile file)
        {
            var binaryReader = new BinaryReader(file.GetStream());
            return binaryReader.ReadBytes((int)file.GetStream().Length);
        }

        public async void MakeAnalysisRequest (MediaFile file)
        {
            var httpClient = new HttpClient();

            // Request headers. Replace the second parameter with a valid subscription key.
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "6f27a2bc82f14b6fa5876368f8209b55");

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "visualFeatures=Description&language=en";
            string uri = "https://api.projectoxford.ai/vision/v1.0/analyze?" + requestParameters;
            //string uri = "https://westus.api.cognitive.microsoft.com/vision/v1.0/analyze?" + requestParameters;
            Debug.WriteLine(uri);

            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await httpClient.PostAsync(uri, content);
                var responseasString = await response.Content.ReadAsStringAsync();

                var dynObject = JsonConvert.DeserializeObject<PhotoDescriptionModel.Rootobject>(responseasString);
                var descriptionStringBuilder = new StringBuilder();
                var captionStringBuilder = new StringBuilder();

                foreach (var tag in dynObject.description.tags)
                {
                    descriptionStringBuilder.Append(tag);
                    descriptionStringBuilder.Append(", ");
                }
                foreach (var descriptionCaption in dynObject.description.captions)
                {
                    captionStringBuilder.Append(descriptionCaption.text);
                    captionStringBuilder.Append(" Confidence: ");
                    captionStringBuilder.Append(descriptionCaption.confidence);
                }
                Description = descriptionStringBuilder.ToString();
                Caption = captionStringBuilder.ToString();
            }
        }

        private async void ReadText (MediaFile file)
        {
            var httpClient = new HttpClient();

            // Request headers. Replace the second parameter with a valid subscription key.
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "6f27a2bc82f14b6fa5876368f8209b55");

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "detectOrientation=true";
            string uri = "https://westus.api.cognitive.microsoft.com/vision/v1.0/ocr?" + requestParameters;
            Debug.WriteLine(uri);

            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await httpClient.PostAsync(uri, content);
                var responseasString = await response.Content.ReadAsStringAsync();
                var dynObject = JsonConvert.DeserializeObject<PhotoTextModel.Rootobject>(responseasString);
                var stringBuilder = new StringBuilder();
                foreach (var region in dynObject.regions)
                {
                    foreach (var line in region.lines)
                    {
                        foreach (var word in line.words)
                        {
                            stringBuilder.Append(word.text);
                            stringBuilder.Append(" ");

                        }
                    }
                }

                PhotoText = stringBuilder.ToString();
            }
        }

        private async Task CancelGroceryItem ()
        {
            await navigation.PopAsync();
        }

        private async Task SaveGroceryItem ()
        {
            var category = await GetCategory();
            var item = new Grocery
            {
                CategoryId = category.Id,
                Name = Name,
                UserId = User
               
            };
            await dbHelper.Insert(item);
            await navigation.PopAsync();
        }

        private async Task<Category> GetCategory ()
        {
            var catList = await dbHelper.Get<Category>();
            Category category;
            var existingCat = catList.FirstOrDefault(c => c.Name == CategoryName);
            if (existingCat == null)
            {
                category = await dbHelper.Insert(new Category() { Name = CategoryName });
            }
            else
            {
                category = existingCat;
            }

            return category;
        }

        public void SetCategory (string item)
        {
            var meats = new List<string>() { "chicken", "steak", "pork" };
            var produce = new List<string>()
            {
                "apple",
                "orange",
                "pear",
                "fruit",
                "strawberries",
                "blueberries",
                "grapes"
            };
            item = item.ToLower().Trim();
            if (item.Contains("bread"))
            {
                CategoryName = "Bakery";
            }
            else if (meats.Any(x => item.Contains(x)))
            {
                CategoryName = "Meat";
            }
            else if (produce.Any(x => item.Contains(x) && !item.Contains("juice")))
            {
                CategoryName = "Produce";
            }
            else
            {
                CategoryName = "Other";

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}