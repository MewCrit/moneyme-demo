


namespace MeoneyMe.Product;

public interface IProductBuilder
{   
        IProductBuilder GetInfo(string product, decimal loan, int terms);

        IProductBuilder IfA();

        IProductBuilder IfB();

        IProductBuilder IfC();

        decimal BuildPayment();
}

