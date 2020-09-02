---
uid: Recore.Collections.Generic.IIterator`1
example:
- *content
---

A common case is calling an API that needs to tell its caller whether it should be called again.
In order to do this with @System.Collections.Generic.IEnumerator`1, the API must look ahead to the next element
with @System.Collections.IEnumerator.MoveNext before returning and then maintain this state between calls.

For example, consider this type that gets status information on a customer's order from some external source:

```cs
using System.Collections.Generic;

class OrderStatusUpdater
{
    private readonly IEnumerator<string> statusEnumerator;

    private string currentStatus = null;

    // We need to look ahead so we know when we've returned the last element
    private string nextStatus = null;
    private bool isFinished = false;

    public OrderStatusUpdater(IEnumerable<string> statuses)
    {
        statusEnumerator = statuses.GetEnumerator();
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        currentStatus = nextStatus;

        if (statusEnumerator.MoveNext())
        {
            nextStatus = statusEnumerator.Current;
        }
        else
        {
            isFinished = true;
        }
    }

    public OrderStatus GetStatusUpdate()
    {
        UpdateStatus();

        return new OrderStatus
        {
            Status = currentStatus,
            IsFinished = isFinished
        };
    }
}
```

Now consider the code with @Recore.Collections.Generic.IIterator`1:

```cs
using System.Collections.Generic;
using Recore.Collections.Generic;

class OrderStatusUpdater
{
    private readonly IIterator<string> statusIterator;

    private string currentStatus = null;

    public OrderStatusUpdater(IEnumerable<string> statuses)
    {
        statusIterator = Iterator.FromEnumerable(statuses);
    }

    public OrderStatus GetStatusUpdate()
    {
        if (statusIterator.HasNext)
        {
            currentStatus = statusIterator.Next();
        }

        return new OrderStatus
        {
            Status = currentStatus,
            IsFinished = !statusIterator.HasNext
        };
    }
}
```