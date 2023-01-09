using ApiRest.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection.PortableExecutable;

namespace ApiRest.Repository
{
    public class RepoMySQLProducts : AbstractRepositoryMySql, IRepositoryProducts
    {
        public RepoMySQLProducts(DataAccess connectionInfo, ILogger<RepoMySQLProducts> log) : 
            base(connectionInfo, log) {}


        //*
        //* CRUD methods
        //*********************************************************************

        public void Create(Product product)
        {
            //SqlConnection sqlConnection = Connect();
            MySqlConnection sqlConnection = Connect();
            MySqlCommand? command = null;

            try
            {
                sqlConnection.Open();
                command = sqlConnection.CreateCommand();

                command.CommandText = "api_rest.create_product";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("Name", MySqlDbType.VarChar, 500).Value = product.Name;
                command.Parameters.Add("Description", MySqlDbType.VarChar, 500).Value = product.Description;
                command.Parameters.Add("Price", MySqlDbType.Float).Value = product.Price;
                command.Parameters.Add("SKU", MySqlDbType.VarChar, 100).Value = product.SKU;
                command.ExecuteNonQuery();

            }
            catch ( Exception ex )
            {
                throw new Exception("An error occurred while adding the product: " + ex.ToString());
            }
            finally
            {
                command?.Dispose();
                sqlConnection.Close(); 
                sqlConnection.Dispose();
            }
        }

        public async Task<Product?> ReadAsync(string sku)
        {
            IEnumerable<Product> products = await FetchProductsAsync(sku);

            if(products?.Count() > 0)
            {
                return await Task.FromResult(products?.ElementAt(0));
            }

            return await Task.FromResult( (Product?)null );
        }

        public async Task<IEnumerable<Product>> ReadAsync(int page, int size)
        {
            IEnumerable<Product> products = await FetchProductsAsync(page, size);

            return products;
        }

        public async Task<IEnumerable<Product>> ReadAsync()
        {
            IEnumerable<Product> products = await FetchProductsAsync();

            return products;
        }

        public void Update(Product product)
        {
            //SqlConnection sqlConnection = Connect();
            MySqlConnection sqlConnection = Connect();
            MySqlCommand? command = null;

            try
            {
                sqlConnection.Open();
                command = sqlConnection.CreateCommand();

                command.CommandText = "api_rest.update_product";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("Name", MySqlDbType.VarChar, 500).Value = product.Name;
                command.Parameters.Add("Description", MySqlDbType.VarChar, 500).Value = product.Description;
                command.Parameters.Add("Price", MySqlDbType.Float).Value = product.Price;
                command.Parameters.Add("SKU", MySqlDbType.VarChar, 100).Value = product.SKU;
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product: " + ex.ToString());
            }
            finally
            {
                command?.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        
        }

        public void Delete(string sku)
        {
            MySqlConnection sqlConnection = Connect();
            MySqlCommand? command = null;

            try
            {
                sqlConnection.Open();
                command = sqlConnection.CreateCommand();

                command.CommandText = "api_rest.delete_product";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("SKU", MySqlDbType.VarChar, 100).Value = sku;
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product: " + ex.ToString());
            }
            finally
            {
                command?.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        //*
        //* Utility methods
        //*********************************************************************


        private async Task<IEnumerable<Product>> FetchProductsAsync(string? sku = null)
        {
            MySqlConnection sqlConnection = Connect();
            MySqlCommand? command = null;
            IEnumerable<Product> result;

            try
            {
                sqlConnection.Open();
                command = sqlConnection.CreateCommand();

                command.CommandText = "api_rest.read_products";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("SKU", MySqlDbType.VarChar, 100).Value = sku;
                result = GetProducts(await command.ExecuteReaderAsync());

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product: " + ex.ToString());
            }
            finally
            {
                command?.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return result;
        }

        private async Task<IEnumerable<Product>> FetchProductsAsync(int page, int size)
        {
            MySqlConnection sqlConnection = Connect();
            MySqlCommand? command = null;
            IEnumerable<Product> result;

            try
            {
                sqlConnection.Open();
                command = sqlConnection.CreateCommand();

                command.CommandText = "api_rest.read_products_paged";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("PAG", MySqlDbType.Int32).Value = page;
                command.Parameters.Add("REG", MySqlDbType.Int32).Value = size;
                result = GetProducts(await command.ExecuteReaderAsync());

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product: " + ex.ToString());
            }
            finally
            {
                command?.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return result;
        }

        private static IEnumerable<Product> GetProducts(DbDataReader reader)
        {
            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                products.Add(new Product()
                {
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    Price = Convert.ToDouble(reader["Price"].ToString()),
                    SKU = reader["SKU"].ToString(),
                    Id = Convert.ToInt16(reader["Id"].ToString()),

                });
            }

            return products;
        }
    }
}
