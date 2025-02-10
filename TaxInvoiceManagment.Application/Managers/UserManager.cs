using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<User>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.ToList();
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} was not found.");
            }
            return user;
        }

        public async Task<bool> CreateUser(User user)
        {            
            await _unitOfWork.Users.AddAsync(user);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateUser(User user)
        {
            await _unitOfWork.Users.UpdateAsync(user);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUser(int id)
        {            
            await _unitOfWork.Users.DeleteAsync(id);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
