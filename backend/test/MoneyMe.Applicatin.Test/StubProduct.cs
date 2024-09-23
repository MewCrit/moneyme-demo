using MeoneyMe.Product;

public class StubProductBuilder : IProductBuilder
{
    private decimal _payment;

    public StubProductBuilder(decimal payment)
    {
        this._payment = payment;

    }

    public decimal BuildPayment()
    {
        return _payment;
    }

    public IProductBuilder GetInfo(string product, decimal loan, int terms)
    {
       return this;
    }

    public IProductBuilder IfA()
    {
        return this;
    }

    public IProductBuilder IfB()
    {
        return this;
    }

    public IProductBuilder IfC()
    {
        return this;
    }
}
