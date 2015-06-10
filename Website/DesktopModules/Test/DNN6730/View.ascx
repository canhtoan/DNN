<%@ Control Language="C#" AutoEventWireup="false" Inherits="Test.DNN6730.View" CodeFile="View.ascx.cs" %>

<%

var key = "APPLICATION_START";
int? portalId = null;
int? pageSize = 50;
int? pageIndex = 0;

using (var dr = DataProvider.Instance().ExecuteReader(
    "GetEventLog", 
    new SqlParameter("@LogTypeKey", key ?? (object)DBNull.Value),
    new SqlParameter("@PortalID", portalId ?? (object)DBNull.Value),
    new SqlParameter("@PageSize", pageSize ?? (object)DBNull.Value),
    new SqlParameter("@PageIndex", pageIndex ?? (object)DBNull.Value)))
{
    dr.NextResult();
    dr.Read();
    Response.Write("Total " + key + " Records: ");
    Response.Write(dr["TotalRecords"]);
}

%>
