namespace _01_FrameWork.Application
{
    public class OperationResultWithId : OperationResult
    {
        public long Id { get; private set; }

        public OperationResultWithId Succedded(long id, string message = "عملیات با موفقیت انجام شد")
        {
            IsSuccedded = true;
            Message = message;
            Id = id;
            return this;
        }

        public new OperationResultWithId Failed(string message)
        {
            IsSuccedded = false;
            Message = message;
            Id = 0;
            return this;
        }
    }
}
