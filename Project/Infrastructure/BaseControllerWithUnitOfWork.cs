using System.Data.Entity;
using System.Linq;

namespace Infrastructure
{
    public class BaseControllerWithUnitOfWork : BaseController
    {

        public BaseControllerWithUnitOfWork()
        {
        }


        private DAL.UnitOfWork _unitOfWork;
        protected virtual DAL.UnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                {
                    _unitOfWork = new DAL.UnitOfWork();
                }
                return (_unitOfWork);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
                _unitOfWork = null;
            }

            base.Dispose(disposing);
        }
    }
}