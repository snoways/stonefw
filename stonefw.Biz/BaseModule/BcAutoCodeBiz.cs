using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using stonefw.Dao.BaseModule;
using stonefw.Entity.BaseModule;
using stonefw.Utility.EntitySql.Data;

namespace stonefw.Biz.BaseModule
{
    public class BcAutoCodeBiz
    {
        private BcAutoCodeDao _dao;
        private BcAutoCodeDao Dao
        {
            get { return _dao ?? (_dao = new BcAutoCodeDao()); }
        }

        public List<BcAutoCodeEntity> GetBcAutoCodeList()
        { return EntityExecution.ReadEntityList<BcAutoCodeEntity>(); }
        public void DeleteBcAutoCode(int id)
        {
            BcAutoCodeEntity entity = new BcAutoCodeEntity() { Id = id };
            EntityExecution.ExecDelete(entity);
        }
        public void AddNewBcAutoCode(BcAutoCodeEntity entity)
        {
            entity.Id = null;
            entity.CurrentDate = DateTime.Now;
            entity.CurrentCode = 0;
            using (var ts = new TransactionScope())
            {
                Dao.ClearDefault(entity.FuncPointId);
                EntityExecution.ExecInsert(entity);
                ts.Complete();
            }
        }
        public void UpdateBcAutoCode(BcAutoCodeEntity entity)
        {
            if (entity.IsDefault == true)
            {
                using (var ts = new TransactionScope())
                {
                    Dao.ClearDefault(entity.FuncPointId);
                    EntityExecution.ExecUpdate(entity);
                    ts.Complete();
                }
            }
            else
            {
                EntityExecution.ExecUpdate(entity);
            }
        }
        public BcAutoCodeEntity GetSingleBcAutoCode(int id) { return EntityExecution.ReadEntity<BcAutoCodeEntity>(n => n.Id == id); }

        public string GetCode(string funcPointId)
        {
            var list = GetBcAutoCodeList().Where(n => n.FuncPointId == funcPointId && n.IsDefault == true).ToList();
            if (list == null || list.Count <= 0) { throw new Exception("请先设置好自动编号规则！"); }
            var entity = list[0];
            if (entity.CurrentDate != DateTime.Now.Date)
            {
                entity.CurrentDate = DateTime.Now.Date;
                entity.CurrentCode = 0;
            }
            entity.CurrentCode += 1;
            var prefix = entity.Prefix;
            var date = DateTime.Parse(entity.CurrentDate.ToString()).ToString(entity.DateFormat);
            var code = entity.CurrentCode.ToString().PadLeft((int)entity.Digit, '0');
            var result = string.Format("{0}{1}{2}", prefix, date, code);
            UpdateBcAutoCode(entity);
            return result;
        }
    }
}
