using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Web.Api.Common.Http
{
      /// <summary>
      ///  Clase que forma la respuesta json
      /// </summary>
    public static class ResponseJson
    {
        /// <summary>
        /// Forma la estrucutra d ela respuesta json
        /// </summary>
        /// <param name="_success"></param>
        /// <param name="_data"></param>
        /// <param name="_message"></param>
        /// <param name="_total"></param
        /// <param name="time" ></param>
        /// <returns></returns>
        public static HttpResponseJson HttpResponse(Boolean _success, object _data = null, String _message = null,int _total=0, string time = null)
        {
            _message = Functions.Functions.ErrorExection(_message);
          

            var _response = new HttpResponseJson
            {
                success = _success,
                message = _message,
                data = _data,
                total=_total,
                date = DateTime.Now.ToString(),
                time = time
            };
                
            return _response;
        }


        public class error
        {
            public int code { get; set; }
            public string message { get; set; }
        }

        [DataContract]
        public abstract class MyResult
        {
            private string _message;
            private bool _success;
            private error _error;

            protected MyResult(bool success, string message)
            {
                this.success = success;
                if (this.success == true)
                {
                    message = "OK";
                }
                this.message = message;
                if (this.success == false)
                {
                    this.error = new error
                    {
                        code = 0,
                        message = this.message
                    };
                }

            }

            protected MyResult(bool success, error error)
            {
                this.success = success;
                if (this.success == true)
                {
                    error = null;
                    this.message = "OK";
                }
                this.error = error;
                if (this.success == false)
                {
                    this.message = this.error.message;
                }
            }

            [DataMember]
            public bool success
            {
                get
                {
                    return _success;
                }
                private set
                {
                    _success = value;
                }
            }

            [DataMember]
            public string message
            {
                get
                {
                    return _message;
                }
                set
                {
                    _message = value;
                }
            }

            [DataMember]
            public error error
            {
                get
                {
                    return _error;
                }
                set
                {
                    _error = value;
                }
            }

        }

        [DataContract]
        [JsonObject]
        public class MyResult<T> : MyResult, IEnumerable<T>
        {
            public MyResult(IEnumerable<T> data, bool success = true, string message = "OK") : base(success, message)
            {
                this.data = data;
            }

            public MyResult(string errorMessage) : base(false, errorMessage) { }
            public MyResult(error error) : base(false, error) { }




            [DataMember]
            public IEnumerable<T> data { get; private set; }


            public IEnumerator<T> GetEnumerator()
            {
                return data.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return data.GetEnumerator();
            }
        }
    }
}