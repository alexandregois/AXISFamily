using System;

namespace family.Domain.Dto
{
	public class ServiceResult<TEntity>
	{
		public String MessageError { get; set; }
		public Boolean IsValid { get; set; }
		public Int32? StatusCode { get; set; }
		public Object RefreshToken { get; set; }
		public TEntity Data { get; set; }

        public static implicit operator ServiceResult<TEntity>(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
