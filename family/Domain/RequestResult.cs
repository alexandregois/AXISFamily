using family.Domain.Dto;

namespace family.Domain
{
	public class RequestResult<TEntity>
	{
		public ServiceResult<TEntity> Result { get; set; }
		public object Id { get; set; }
		public object Status { get; set; }
		public object IsCanceled { get; set; }
		public object IsCompleted { get; set; }
		public object CreateOptions { get; set; }
		public object IsFaulted { get; set; }
	}
}
