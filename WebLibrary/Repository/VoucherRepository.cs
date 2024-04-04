using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.DAO;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        public bool checkCode(string codeInput) => VoucherDAO.Instance.checkCode(codeInput);

        public void CreateVoucher(Voucher voucher) => VoucherDAO.Instance.CreateVoucher(voucher);

        public bool CreateVoucherBool(Voucher voucher) => VoucherDAO.Instance.CreateVoucherBool(voucher);
    
        public string GenerateRandomCode() => VoucherDAO.Instance.GenerateRandomCode();

        public Voucher GetVoucherByCode(string codeVoucher) => VoucherDAO.Instance.GetVoucherByCode(codeVoucher);

        public Voucher GetVoucherByID(int id) => VoucherDAO.Instance.GetVoucherByID(id);

        public void Remove(int VoucherID) => VoucherDAO.Instance.Remove(VoucherID);

        public void Update(Voucher voucher) => VoucherDAO.Instance.Update(voucher);

        public IEnumerable<Voucher> VouchersList() => VoucherDAO.Instance.VouchersList();

    }
}