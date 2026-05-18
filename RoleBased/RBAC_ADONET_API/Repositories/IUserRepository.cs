using RBAC_ADONET_API.DTOs;
using RBAC_ADONET_API.Models;

namespace RBAC_ADONET_API.Repositories
{
    public interface IUserRepository
    {
        User Login(string email, string password);

        List<User> GetUsers();

        void CreateUser(User user);
        List<Permission> GetPermissions(int userId);

        List<Order> GetOrders();

        void CreateOrder(Order order);

        void UpdateOrder(Order order);

        void DeleteOrder(int id);

        List<Order> GetOrdersByUser(int userId);

        PagedResult<Order> GetOrdersPaged( int pageNumber, int pageSize);

        List<Order> SearchOrders(string search, decimal? minPrice, decimal? maxPrice);

        DashboardDto GetDashboardReport();


    }
}
