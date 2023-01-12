namespace active_mq_app.Amq
{
    public interface ITypedConsumer<in T>
    {
        public Task ConsumeAsync(T message, CancellationToken cancellationToken);
    }
}