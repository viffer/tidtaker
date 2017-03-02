namespace UtleiraTidtaker.DataReader.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;

    public class ExcelRepository : IDisposable
    {
        private readonly string _path;
        private readonly OleDbConnection _connection;

        public ExcelRepository(string path)
        {
            _path = path;
            // http://www.microsoft.com/en-us/download/confirmation.aspx?id=13255
            _connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={_path};Extended Properties='Excel 8.0;HDR=YES;';");
        }

        public IEnumerable<string> GetSheetNames()
        {
            OpenConnection();
            using (var schemaTable = _connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
            {
                foreach (DataRow row in schemaTable.Rows)
                {
                    yield return row["TABLE_NAME"].ToString();
                }
            }
        }

        public DataTable Load(string sheetName)
        {
            OpenConnection();
            using (var command = new OleDbCommand(string.Format("Select * From [{0}]", sheetName), _connection))
            {
                using (var sda = new OleDbDataAdapter(command))
                {
                    var data = new DataTable();
                    sda.Fill(data);
                    return data;
                }
            }
        }

        private void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public DateTime GetFiletime()
        {
            var fileinfo = new FileInfo(_path);
            return fileinfo.CreationTime;
        }
    }
}
