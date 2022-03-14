using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ADO_Example.Models;

namespace ADO_Example.DLA
{
    public class ProductDAL
    {
        string conString=ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();

        //Get All Products

        public List<Product> GetAllProdcuts()
        {
            List<Product> productlist = new List<Product>();


            using(SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.CommandText = "sp_GetAllProducts";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts=new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();


                foreach(DataRow dr  in dtProducts.Rows)
                {
                    productlist.Add(new Product
                    {
                        ProductID=Convert.ToInt32(dr["ProductId"]),
                        ProductName=dr["ProductName"].ToString(),
                        Price=Convert.ToDecimal(dr["Price"]),
                        Qty=Convert.ToInt32(dr["qty"]),
                        Remarks=dr["Remarks"].ToString()

                    });

                }
            }
            return productlist;
        }

        //Insert Products
        
        public bool InsertProducts(Product product)
        {
            int id = 0;
            using(SqlConnection connection=new SqlConnection(conString))
            {
                SqlCommand command=new SqlCommand("sp_InsertProducts",connection);
                command.CommandType=CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductName",product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);


                connection.Open();
                id=command.ExecuteNonQuery();
                connection.Close();
            }
            if(id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Product> GetProdcutsByID(int ProductID)
        {
            List<Product> productlist = new List<Product>();


            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.CommandText = "sp_GetProductByID";
                command.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();


                foreach (DataRow dr in dtProducts.Rows)
                {
                    productlist.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["qty"]),
                        Remarks = dr["Remarks"].ToString()

                    });

                }
            }
            return productlist;
        }


        public bool UpdateProducts(Product product)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);


                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Delete
        public string DeleteProduct(int productid)
        {
            string result = "";

            using (SqlConnection connection=new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteProduct", connection); 
                command.CommandType=CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", productid);
                command.Parameters.Add("@OutputMessage", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@OutputMessage"].Value.ToString();
                connection.Close();
            }
            return result;
        }
    }
}