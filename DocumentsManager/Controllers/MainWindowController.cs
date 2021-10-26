using DocumentsManager.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DocumentsManager.Controllers
{
    public class MainWindowController
    {
        private const string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=DocumentsManagerDB;Data Source=(localdb)\\MSSQLLocalDB";

        public bool SaveDocument(DocumentHeader document)
        {
            SqlTransaction transaction = null;
            FillDocumentIds(document);

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();
                transaction = connection.BeginTransaction();
                string cmdTxt1 = "INSERT INTO [dbo].[DocumentHeader] (Id, Date, ClientNumber, Name, NetPrice, GrossPrice) " +
                                "VALUES (@Id, @Date, @ClientNumber, @Name, @NetPrice, @GrossPrice)";

                SqlCommand headerCommand = new SqlCommand(cmdTxt1, connection, transaction);
                headerCommand.Parameters.AddWithValue("@Id", document.Id);
                headerCommand.Parameters.AddWithValue("@Date", document.Date);
                headerCommand.Parameters.AddWithValue("@ClientNumber", document.ClientNumber);
                headerCommand.Parameters.AddWithValue("@Name", document.Name);
                headerCommand.Parameters.AddWithValue("@NetPrice", document.NetPrice);
                headerCommand.Parameters.AddWithValue("@GrossPrice", document.GrossPrice);
                headerCommand.ExecuteNonQuery();

                string cmdTxt2 = "INSERT INTO [dbo].[DocumentItems] (Id, ArtName, Quantity, NetPrice, GrossPrice, DocumentHeaderId) " +
                    "VALUES (@Id, @ArtName, @Quantity, @NetPrice, @GrossPrice, @DocumentHeaderId)";

                SqlCommand itemsCommand = new SqlCommand(cmdTxt2, connection, transaction);
                itemsCommand.Parameters.AddWithValue("@Id", null);
                itemsCommand.Parameters.AddWithValue("@ArtName", null);
                itemsCommand.Parameters.AddWithValue("@Quantity", null);
                itemsCommand.Parameters.AddWithValue("@NetPrice", null);
                itemsCommand.Parameters.AddWithValue("@GrossPrice", null);
                itemsCommand.Parameters.AddWithValue("@DocumentHeaderId", null);

                for (int i = 0; i < document.DocumentItems.Count; i++)
                {
                    DocumentItem item = document.DocumentItems[i];
                    itemsCommand.Parameters["@Id"].Value = item.Id;
                    itemsCommand.Parameters["@ArtName"].Value = item.ArtName;
                    itemsCommand.Parameters["@Quantity"].Value = item.Quantity;
                    itemsCommand.Parameters["@NetPrice"].Value = item.NetPrice;
                    itemsCommand.Parameters["@GrossPrice"].Value = item.GrossPrice;
                    itemsCommand.Parameters["@DocumentHeaderId"].Value = document.Id;

                    itemsCommand.ExecuteNonQuery();
                }

                transaction.Commit();
                connection.Close();
                connection.Dispose();

            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();

                MessageBox.Show(string.Concat("Wystąpił błąd: ", ex.Message));
                return false;
            }

            return true;
        }

        public void UpdateDocument(string tableName, string columnName, int id, object newValue)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string cmdTxt = "UPDATE [dbo].[" + tableName + "] SET " + columnName + " = @NewValue " +
                    "WHERE Id = " + id;

                SqlCommand updateCommand = new SqlCommand(cmdTxt, connection);
                updateCommand.Parameters.AddWithValue("@NewValue", newValue);
                updateCommand.ExecuteNonQuery();

                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat("Wystąpił błąd: ", ex.Message));
            }
        }

        public void DeleteDocument(int id)
        {
            SqlTransaction transaction = null;
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                string cmdTxt1 = "DELETE FROM [dbo].[DocumentItems] WHERE DocumentHeaderId = " + id;
                SqlCommand itemsCommand = new SqlCommand(cmdTxt1, connection, transaction);
                itemsCommand.ExecuteNonQuery();

                string cmdTxt2 = "DELETE FROM [dbo].[DocumentHeader] WHERE Id = " + id;
                SqlCommand headerCommand = new SqlCommand(cmdTxt2, connection, transaction);
                headerCommand.ExecuteNonQuery();

                transaction.Commit();
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();

                MessageBox.Show(string.Concat("Wystąpił błąd: ", ex.Message));
            }
        }

        public DataSet ReadData()
        {
            DataSet data = new DataSet();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    SqlDataAdapter headerDataAdapter = new
                        SqlDataAdapter("SELECT Id, Date, ClientNumber, Name, NetPrice, GrossPrice FROM dbo.DocumentHeader", connection);
                    headerDataAdapter.Fill(data, "Headers");

                    SqlDataAdapter itemsDataAdapter = new
                        SqlDataAdapter("SELECT Id, ArtName, Quantity, NetPrice, GrossPrice, DocumentHeaderId FROM DocumentItems", connection);
                    itemsDataAdapter.Fill(data, "Items");

                    DataRelation relation = new DataRelation("DocumentItems",
                        data.Tables["Headers"].Columns["Id"],
                        data.Tables["Items"].Columns["DocumentHeaderId"]);
                    data.Relations.Add(relation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat("Wystąpił błąd: ", ex.Message));
            }

            return data;
        }

        private void FillDocumentIds(DocumentHeader document)
        {
            document.Id = GetNextHeaderId();
            int nextItemId = GetNextItemId();

            for (int i = 0; i < document.DocumentItems.Count; i++)
            {
                document.DocumentItems[i].Id = nextItemId++;
            }
        }

        private int GetNextHeaderId()
        {
            return GetNextId(DocumentHeader.TableName);
        }

        private int GetNextItemId()
        {
            return GetNextId(DocumentItem.TableName);
        }

        private int GetNextId(string tableName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT MAX(Id) FROM " + tableName, connection);

                    object value = command.ExecuteScalar();

                    if (value == DBNull.Value)
                        return 1;

                    int maxId = (int)value;
                    return maxId + 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat("Wystąpił błąd: ", ex.Message));
            }

            return 0;
        }
    }
}
