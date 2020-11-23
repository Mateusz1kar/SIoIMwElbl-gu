using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PracaDyplomowa.Models
{
    class Facebook
    {
        readonly string _accessToken;
        readonly string _pageID;
        readonly string _facebookAPI = "https://graph.facebook.com/";
        readonly string _pageEdgeFeed = "feed";
        readonly string _pageEdgePhotos = "photos";
        readonly string _postToPageURL;
        readonly string _postToPagePhotosURL;

        public Facebook(string accessToken, string pageID)
        {
            _accessToken = accessToken;
            _pageID = pageID;
            _postToPageURL = $"{_facebookAPI}{pageID}/{_pageEdgeFeed}";
            _postToPagePhotosURL = $"{_facebookAPI}{pageID}/{_pageEdgePhotos}";
        }

        /// <summary>
        /// Publish a simple text post
        /// </summary>
        /// <returns>StatusCode and JSON response</returns>
        /// <param name="postText">Text for posting</param>
        public async Task<Tuple<int, string>> PublishSimplePost(string postText)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                { "access_token", _accessToken },
                { "message", postText }//,
                // { "formatting", "MARKDOWN" } // doesn't work
            };

                var httpResponse = await http.PostAsync(
                    _postToPageURL,
                    new FormUrlEncodedContent(postData)
                    );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                    );
            }
        }

        /// <summary>
        /// Publish a post to Facebook page
        /// </summary>
        /// <returns>Result</returns>
        /// <param name="postText">Post to publish</param>
        /// <param name="pictureURL">Post to publish</param>
        public string PublishToFacebook(string postText, string pictureURL)
        {
            try
            {
                // upload picture first
                var rezImage = Task.Run(async () =>
                {
                    using (var http = new HttpClient())
                    {
                        return await UploadPhoto(pictureURL);
                    }
                });
                var rezImageJson = JObject.Parse(rezImage.Result.Item2);

                if (rezImage.Result.Item1 != 200)
                {
                    try // return error from JSON
                    {
                        return $"Error uploading photo to Facebook. {rezImageJson["error"]["message"].Value<string>()}";
                    }
                    catch (Exception ex) // return unknown error
                    {
                        // log exception somewhere
                        return $"Unknown error uploading photo to Facebook. {ex.Message}";
                    }
                }
                // get post ID from the response
                string postID = rezImageJson["post_id"].Value<string>();

                // and update this post (which is actually a photo) with your text
                var rezText = Task.Run(async () =>
                {
                    using (var http = new HttpClient())
                    {
                        return await UpdatePhotoWithPost(postID, postText);
                    }
                });
                var rezTextJson = JObject.Parse(rezText.Result.Item2);

                if (rezText.Result.Item1 != 200)
                {
                    try // return error from JSON
                    {
                        return $"Error posting to Facebook. {rezTextJson["error"]["message"].Value<string>()}";
                    }
                    catch (Exception ex) // return unknown error
                    {
                        // log exception somewhere
                        return $"Unknown error posting to Facebook. {ex.Message}";
                    }
                }

                return "OK";
            }
            catch (Exception ex)
            {
                // log exception somewhere
                return $"Unknown error publishing post to Facebook. {ex.Message}";
            }
        }

        //public string PublishImageListToFacebook(string postText, List<string> pictureURL)
        //{
        //    try
        //    {
        //        // upload picture first
        //        var rezImage = Task.Run(async () =>
        //        {
        //            using (var http = new HttpClient())
        //            {

        //                return await UploadPhotoList(pictureURL);
        //            }
        //        });
        //        var rezImageJson = JObject.Parse(rezImage.Result[0].Item2);
        //        for (int i = 1; i < rezImage.Result.Count(); i++)
        //        {
        //            rezImageJson.Add( JObject.Parse(rezImage.Result[i].Item2));
        //        }

        //        //if (rezImage.Result.Item1 != 200)
        //        //{
        //        //    try // return error from JSON
        //        //    {
        //        //        return $"Error uploading photo to Facebook. {rezImageJson["error"]["message"].Value<string>()}";
        //        //    }
        //        //    catch (Exception ex) // return unknown error
        //        //    {
        //        //        // log exception somewhere
        //        //        return $"Unknown error uploading photo to Facebook. {ex.Message}";
        //        //    }
        //        //}
        //        // get post ID from the response
        //        string postID = rezImageJson["post_id"].Value<string>();

        //        // and update this post (which is actually a photo) with your text
        //        var rezText = Task.Run(async () =>
        //        {
        //            using (var http = new HttpClient())
        //            {
        //                return await UpdatePhotoWithPost(postID, postText);
        //            }
        //        });
        //        var rezTextJson = JObject.Parse(rezText.Result.Item2);

        //        if (rezText.Result.Item1 != 200)
        //        {
        //            try // return error from JSON
        //            {
        //                return $"Error posting to Facebook. {rezTextJson["error"]["message"].Value<string>()}";
        //            }
        //            catch (Exception ex) // return unknown error
        //            {
        //                // log exception somewhere
        //                return $"Unknown error posting to Facebook. {ex.Message}";
        //            }
        //        }

        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        // log exception somewhere
        //        return $"Unknown error publishing post to Facebook. {ex.Message}";
        //    }
        //}


        public string PublishImageListToFacebook(string postText, List<string> pictureURL)
        {
            try
            {
                // upload picture first
                var rezImage = Task.Run(async () =>
                {
                    using (var http = new HttpClient())
                    {

                        return await UploadPhotoList(pictureURL);
                    }
                });
                var rezImageJson = JObject.Parse(rezImage.Result.Item2);

                if (rezImage.Result.Item1 != 200)
                {
                    try // return error from JSON
                    {
                        return $"Error uploading photo to Facebook. {rezImageJson["error"]["message"].Value<string>()}";
                    }
                    catch (Exception ex) // return unknown error
                    {
                        // log exception somewhere
                        return $"Unknown error uploading photo to Facebook. {ex.Message}";
                    }
                }
                // get post ID from the response
                string postID = rezImageJson["post_id"].Value<string>();

                // and update this post (which is actually a photo) with your text
                var rezText = Task.Run(async () =>
                {
                    using (var http = new HttpClient())
                    {
                        return await UpdatePhotoWithPost(postID, postText);
                    }
                });
                var rezTextJson = JObject.Parse(rezText.Result.Item2);

                if (rezText.Result.Item1 != 200)
                {
                    try // return error from JSON
                    {
                        return $"Error posting to Facebook. {rezTextJson["error"]["message"].Value<string>()}";
                    }
                    catch (Exception ex) // return unknown error
                    {
                        // log exception somewhere
                        return $"Unknown error posting to Facebook. {ex.Message}";
                    }
                }

                return "OK";
            }
            catch (Exception ex)
            {
                // log exception somewhere
                return $"Unknown error publishing post to Facebook. {ex.Message}";
            }
        }


        /// <summary>
        /// Upload a picture (photo)
        /// </summary>
        /// <returns>StatusCode and JSON response</returns>
        /// <param/* name="photoURL*/">URL of the picture to upload</param>
        public async Task<Tuple<int, string>> UploadPhoto(string photoURL)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                { "access_token", _accessToken },
                { "url", photoURL }
            };

                var httpResponse = await http.PostAsync(
                    _postToPagePhotosURL,
                    new FormUrlEncodedContent(postData)
                    );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                    );
            }
        }

        /// <summary>
        /// Upload a picture (photo)
        /// </summary>
        /// <returns>StatusCode and JSON response</returns>
        /// <param/* name="photoURL*/">URL of the picture to upload</param>
        ///                                                                                                 tyle postów ile zdjęć
        //public async Task<List<Tuple<int, string>>> UploadPhotoList(List<string>photoURL)
        //{
        //    using (var http = new HttpClient())
        //    {
        //        List<string> httpContentList = new List<string>();
        //        List<int> StatudCodetList = new List<int>();
        //        List<Tuple<int, string>> tupleList = new List<Tuple<int, string>>();
        //        for (int i = 0; i < photoURL.Count(); i++)
        //        {
        //            var postData = new Dictionary<string, string> {
        //                { "access_token", _accessToken },
        //                { "url", photoURL[i] }
        //            };

        //            var httpResponse = await http.PostAsync(
        //            _postToPagePhotosURL,
        //            new FormUrlEncodedContent(postData)
        //            );
        //            //httpContentList.Add(await httpResponse.Content.ReadAsStringAsync());
        //            //StatudCodetList.Add((int)httpResponse.StatusCode);
        //            var httpContent = await httpResponse.Content.ReadAsStringAsync();
        //            tupleList.Add(new Tuple<int, string>(
        //            (int)httpResponse.StatusCode,
        //                httpContent
        //            ));
        //        }


        //        return tupleList;
        //    }
        //}

        public async Task<Tuple<int, string>> UploadPhotoList(List<string> photoURL)
        {
            using (var http = new HttpClient())
            {

                List<string> httpContentList = new List<string>();
                List<int> StatudCodetList = new List<int>();
                var postData = new Dictionary<string, string>();
                postData.Add("access_token", _accessToken);
                List<Tuple<int, string>> tupleList = new List<Tuple<int, string>>();
                postData.Add("url" , photoURL[0]);
                postData.Add("url2" , photoURL[1]);

                //for (int i = 0; i < photoURL.Count(); i++)
                //{
                //    postData.Add("url"+i, photoURL[i]);
                //    };


                var httpResponse = await http.PostAsync(
                      _postToPagePhotosURL,
                         new FormUrlEncodedContent(postData)
                    );;
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();
                return new Tuple<int, string>(
                 (int)httpResponse.StatusCode,
                 httpContent
                 );

            }


            
        }
        

        /// <summary>
        /// Update the uploaded picture (photo) with the given text
        /// </summary>
        /// <returns>StatusCode and JSON response</returns>
        /// <param name="postID">Post ID</param>
        /// <param name="postText">Text to add tp the post</param>
        public async Task<Tuple<int, string>> UpdatePhotoWithPost(string postID, string postText)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                { "access_token", _accessToken },
                { "message", postText }
            };

                var httpResponse = await http.PostAsync(
                    $"{_facebookAPI}{postID}",
                    new FormUrlEncodedContent(postData)
                    );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                    );
            }
        }
    }
}
