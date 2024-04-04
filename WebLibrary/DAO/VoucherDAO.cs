using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.DAO
{
    public class VoucherDAO
    {
        private static VoucherDAO instance = null;
        private static readonly object instanceLock = new object();
        public static VoucherDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new VoucherDAO();
                    }
                    return instance;
                }
            }
        }

        public Voucher GetVoucherByID(int id)
        {
            Voucher voucher = null;
            try
            {
                using var context = new DBContext();
                voucher = context.Vouchers.SingleOrDefault(c => c.VoucherId.Equals(id));

            }
            catch (System.Exception)
            {

                throw;
            }
            return voucher;
        }

        public Voucher GetVoucherByCode(string codeVoucher)
        {
            Voucher voucher = null;
            try
            {
                using var context = new DBContext();
                voucher = context.Vouchers.SingleOrDefault(c => c.CodeVoucher.Equals(codeVoucher));
            }
            catch (System.Exception)
            {
                throw;
            }
            return voucher;
        }

        public void CreateVoucher(Voucher voucher)
        {
            try
            {
                Voucher existingVoucher = GetVoucherByID(voucher.VoucherId);
                if (existingVoucher == null)
                {
                    using (var context = new DBContext())
                    {
                        context.Vouchers.Add(voucher);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Voucher already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
      

        public bool CreateVoucherBool(Voucher voucher)
        {
            try
            {
                Voucher existingVoucher = GetVoucherByID(voucher.VoucherId);
                if (existingVoucher == null)
                {
                    using (var context = new DBContext())
                    {
                        context.Vouchers.Add(voucher);
                        context.SaveChanges();
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Voucher> VouchersList()
        {
            var voucher = new List<Voucher>();
            try
            {
                using var context = new DBContext();
                voucher = context.Vouchers.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return voucher;
        }

        public void Update(Voucher voucher)
        {
            try
            {
                Voucher existingVoucher = GetVoucherByID(voucher.VoucherId);
                if (existingVoucher != null)
                {
                    using (var context = new DBContext())
                    {
                        context.Vouchers.Update(voucher);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Voucher does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(int VoucherID)
        {
            try
            {
                Voucher Voucher = GetVoucherByID(VoucherID);
                if (Voucher != null)
                {
                    using (var context = new DBContext())
                    {
                        context.Vouchers.Remove(Voucher);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Voucher does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] codeArray = new char[10];

            for (int i = 0; i < 10; i++)
            {
                codeArray[i] = chars[random.Next(chars.Length)];
            }

            return new string(codeArray);
        }


        public bool checkCode(string codeInput)
        {
            try
            {
                var list = VouchersList();

                foreach (var item in list)
                {
                    if (codeInput.ToUpper() == item.CodeVoucher) return true;
                }
                return false;
            }
            catch (System.Exception)
            {
                throw;
            }
        }




    }
}