using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace UtleiraTidtaker.DataReader.Repository
{
    public class ExcelRepository : IDisposable
    {
        private readonly string _path;
        private readonly string _connectionString;
        private readonly OleDbConnection _connection;

        public ExcelRepository(string path)
        {
            _path = path;
            _connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;';", _path);
            _connection = new OleDbConnection(_connectionString);
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
