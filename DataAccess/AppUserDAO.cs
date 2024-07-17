using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AppUserDAO
    {
        private static AppUserDAO instance;
        public static AppUserDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppUserDAO();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<AppUser>> GetAppUsers()
        {
            var _context = new PostDBContext();
            var list = await _context.AppUsers.ToListAsync();
            return list;
        }

        public async Task<AppUser> GetAppUserById(int id)
        {
            var _context = new PostDBContext();
            var product = await _context.AppUsers.SingleOrDefaultAsync(s => s.UserId == id);
            return product;
        }

        public async Task Add(AppUser pro)
        {
            var _db = new PostDBContext();
            _db.AppUsers.Add(pro);
            await _db.SaveChangesAsync();
        }

        public async Task Update(AppUser pro)
        {
            var _db = new PostDBContext();
            var o = await GetAppUserById(pro.UserId);
            if (o != null)
            {
                _db.AppUsers.Update(pro);
                await _db.SaveChangesAsync();
            }

        }

        public async Task Delete(AppUser pro)
        {
            var _db = new PostDBContext();
            var o = await GetAppUserById(pro.UserId);
            if (o != null)
            {
                _db.AppUsers.Remove(pro);
                await _db.SaveChangesAsync();
            }

        }
    }
}

