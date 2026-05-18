using Microsoft.Data.SqlClient;
using RBAC_ADONET_API.DTOs;
using RBAC_ADONET_API.Models;
using System.Data;

namespace RBAC_ADONET_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;

        public UserRepository(IConfiguration config)
        {
            connectionString =
                config.GetConnectionString("DefaultConnection");
        }

        public User Login(string email, string password)
        {
            User user = null;

            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand("sp_LoginUser", con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", email);

            cmd.Parameters.AddWithValue("@Password", password);

            con.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            if (reader.Read())
            {
                user = new User
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["Password"].ToString(),
                    RoleId = Convert.ToInt32(reader["RoleId"])
                };
            }

            return user;
        }

        public List<User> GetUsers()
        {
            List<User> users = new();

            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand("sp_GetUsers", con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            con.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["Password"].ToString(),
                    RoleId = Convert.ToInt32(reader["RoleId"])
                });
            }

            return users;
        }

        public void CreateUser(User user)
        {
            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand("sp_CreateUser", con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", user.Name);

            cmd.Parameters.AddWithValue("@Email", user.Email);

            cmd.Parameters.AddWithValue("@Password", user.Password);

            cmd.Parameters.AddWithValue("@RoleId", user.RoleId);

            con.Open();

            cmd.ExecuteNonQuery();
        }


        public List<Permission> GetPermissions(int userId)
        {
            List<Permission> permissions = new();

            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand(
                    "sp_GetPermissionsByUserId",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@UserId",
                userId);

            con.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                permissions.Add(new Permission
                {
                    PermissionName =
                        reader["PermissionName"].ToString()
                });
            }

            return permissions;
        }

        public List<Order> GetOrders()
        {
            List<Order> orders = new();

            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand("sp_GetOrders", con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            con.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order
                {
                    Id = Convert.ToInt32(reader["Id"]),

                    ProductName =
                        reader["ProductName"].ToString(),

                    Quantity =
                        Convert.ToInt32(reader["Quantity"]),

                    Price =
                        Convert.ToDecimal(reader["Price"]),

                    UserId =
                        Convert.ToInt32(reader["UserId"])
                });
            }

            return orders;
        }


        public void CreateOrder(Order order)
        {
            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand("sp_CreateOrder", con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@ProductName",
                order.ProductName);

            cmd.Parameters.AddWithValue(
                "@Quantity",
                order.Quantity);

            cmd.Parameters.AddWithValue(
                "@Price",
                order.Price);

            cmd.Parameters.AddWithValue(
                "@UserId",
                order.UserId);

            con.Open();

            cmd.ExecuteNonQuery();
        }


        public void UpdateOrder(Order order)
        {
            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand("sp_UpdateOrder", con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@Id",
                order.Id);

            cmd.Parameters.AddWithValue(
                "@ProductName",
                order.ProductName);

            cmd.Parameters.AddWithValue(
                "@Quantity",
                order.Quantity);

            cmd.Parameters.AddWithValue(
                "@Price",
                order.Price);

            con.Open();

            cmd.ExecuteNonQuery();
        }


        public void DeleteOrder(int id)
        {
            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand("sp_DeleteOrder", con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@Id",
                id);

            con.Open();

            cmd.ExecuteNonQuery();
        }


        public List<Order> GetOrdersByUser(int userId)
        {
            List<Order> orders = new();

            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand(
                    "sp_GetOrdersByUser",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@UserId",
                userId);

            con.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order
                {
                    Id = Convert.ToInt32(reader["Id"]),

                    ProductName =
                        reader["ProductName"].ToString(),

                    Quantity =
                        Convert.ToInt32(reader["Quantity"]),

                    Price =
                        Convert.ToDecimal(reader["Price"]),

                    UserId =
                        Convert.ToInt32(reader["UserId"])
                });
            }

            return orders;
        }

        public PagedResult<Order> GetOrdersPaged(
    int pageNumber,
    int pageSize)
        {
            List<Order> orders = new();

            int totalCount = 0;

            using SqlConnection con =
                new SqlConnection(connectionString);

            con.Open();

            

            using SqlCommand cmd =
                new SqlCommand(
                    "sp_GetOrdersPaged",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@PageNumber",
                pageNumber);

            cmd.Parameters.AddWithValue(
                "@PageSize",
                pageSize);

            SqlDataReader reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order
                {
                    Id =
                        Convert.ToInt32(reader["Id"]),

                    ProductName =
                        reader["ProductName"].ToString(),

                    Quantity =
                        Convert.ToInt32(reader["Quantity"]),

                    Price =
                        Convert.ToDecimal(reader["Price"]),

                    UserId =
                        Convert.ToInt32(reader["UserId"])
                });
            }

            reader.Close();

            

            using SqlCommand countCmd =
                new SqlCommand(
                    "sp_GetOrdersCount",
                    con);

            countCmd.CommandType =
                CommandType.StoredProcedure;

            totalCount =
                Convert.ToInt32(
                    countCmd.ExecuteScalar());

            return new PagedResult<Order>
            {
                TotalCount = totalCount,

                Items = orders
            };



        }


        public List<Order> SearchOrders(string search, decimal? minPrice, decimal? maxPrice)
        {
            List<Order> orders = new();

            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand(
                    "sp_SearchOrders",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@Search",
                (object?)search ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@MinPrice",
                (object?)minPrice ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@MaxPrice",
                (object?)maxPrice ?? DBNull.Value);

            con.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order
                {
                    Id =
                        Convert.ToInt32(reader["Id"]),

                    ProductName =
                        reader["ProductName"].ToString(),

                    Quantity =
                        Convert.ToInt32(reader["Quantity"]),

                    Price =
                        Convert.ToDecimal(reader["Price"]),

                    UserId =
                        Convert.ToInt32(reader["UserId"])
                });
            }

            return orders;
        }


        public DashboardDto GetDashboardReport()
        {
            DashboardDto dashboard = new();

            using SqlConnection con =
                new SqlConnection(connectionString);

            using SqlCommand cmd =
                new SqlCommand(
                    "sp_DashboardReport",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            con.Open();

            SqlDataReader reader =
                cmd.ExecuteReader();

            if (reader.Read())
            {
                dashboard.TotalOrders =
                    Convert.ToInt32(reader["TotalOrders"]);

                dashboard.TotalSales =
                    Convert.ToDecimal(reader["TotalSales"]);

                dashboard.TotalUsers =
                    Convert.ToInt32(reader["TotalUsers"]);
            }

            return dashboard;
        }



    }
}