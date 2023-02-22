using System;

namespace AdicionalWeb.Entidades
{
    [Serializable]
    public class AdicionalResponse
    {
        public AdicionalResponse()
        {
            this.Message = string.Empty;
            this.ExceptionMessage = string.Empty;
        }

        public string Message { get; set; }

        public bool IsFaulted { get; set; }

        public object Result { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
