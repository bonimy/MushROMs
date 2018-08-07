namespace Helper
{
    using System;

    public interface IExceptionHandler
    {
        void ShowException(Exception ex);

        bool ShowExceptionAndRetry(Exception ex);
    }
}
