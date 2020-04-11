using GPRO.Ultilities;
using HMS.Data.Model;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLAccount
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLAccount _Instance;  //volatile =>  tranh dung thread
        public static BLLAccount Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLAccount();

                return _Instance;
            }
        }
        private BLLAccount() { }
        #endregion

        public ModelSelectItem FindAccount(string connectionString, string taikhoan, string matkhau)
        {
            using (var db = new HMSEntities(connectionString))
            {
                string hasPass = GlobalFunction.EncryptMD5(matkhau);
                return db.H_Account.Where(x => x.AccName.Trim() == taikhoan.Trim() && hasPass == x.Password).Select(acc => new ModelSelectItem() { Id = acc.Id, Code = acc.AccName, Data = acc.Role, Name = acc.H_NhanVien.Name, Data1 = acc.Avatar }).FirstOrDefault();
            }
        }

        public AccountModel FindAccount(string connectionString, int accId)
        {
            AccountModel accountModel = null;
            using (var db = new HMSEntities(connectionString))
            {
                var acc = db.H_Account.FirstOrDefault(x => !x.IsDeleted && !x.IsAdmin && x.Id == accId);
                if (acc != null)
                {
                    accountModel = new AccountModel();
                    accountModel.Id = acc.Id;
                    accountModel.TenTK = acc.AccName;
                    accountModel.Avatar = acc.Avatar;
                    if (acc.H_NhanVien != null)
                    {
                        accountModel.HoTen = acc.H_NhanVien.Name;
                        accountModel.DienThoai = acc.H_NhanVien.Phone;
                        accountModel.DiaChi = acc.H_NhanVien.Address;
                        accountModel.Note = acc.H_NhanVien.Note;
                        accountModel.UserId = acc.NhanVienId;
                        accountModel.ChucVu = acc.H_NhanVien.H_LoaiNhanVien.Code;
                    }
                }
            }
            return accountModel;
        }

        public bool UpAvatar(string connectString, int accId, string fileName)
        {
            using (var db = new HMSEntities(connectString))
            {
                var obj = db.H_Account.FirstOrDefault(x => !x.IsDeleted && x.Id == accId);
                if (obj != null)
                {
                    obj.Avatar = fileName;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public H_Account Get(string connectString, int accId )
        {
            using (var db = new HMSEntities(connectString))
            {
                return db.H_Account.FirstOrDefault(x => !x.IsDeleted && x.Id == accId); 
            }
        }

        public bool UpdatePassword(string connectString, int accId, string password)
        {
            using (var db = new HMSEntities(connectString))
            {
                var acc = Get(connectString, accId);
                if (acc != null)
                {
                    acc.Password = GlobalFunction.EncryptMD5(password);
                    db.Entry<H_Account>(acc).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

         
    }
}
