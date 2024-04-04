using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public interface IVoucherRepository
    {
        Voucher GetVoucherByID(int id);
        void CreateVoucher(Voucher voucher);
        IEnumerable<Voucher> VouchersList();
        void Update(Voucher voucher);
        void Remove(int VoucherID);
        string GenerateRandomCode();
        bool checkCode(string codeInput);
        Voucher GetVoucherByCode(string codeVoucher);
        bool CreateVoucherBool(Voucher voucher);
    }
}