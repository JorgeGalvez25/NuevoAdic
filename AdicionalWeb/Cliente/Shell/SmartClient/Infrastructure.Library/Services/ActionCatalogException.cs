//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by this guidance package as part of the solution template
//
// The ActionCatalogException class is the exception used by the ActionCatalogService
// 
// For more information see: 
// ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.scsf.2007may/SCSF/html/03-01-010-How_to_Create_Smart_Client_Solutions.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using System;

namespace EstandarCliente.Infrastructure.Library.Services
{
    [Serializable]
    public class ActionCatalogException : Exception
    {
        public ActionCatalogException()
            : base()
        {
        }

        public ActionCatalogException(string message)
            : base(message)
        {

        }

        public ActionCatalogException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
