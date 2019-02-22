using System;
using System.Collections.Generic;
using System.Linq;

namespace JetCommander
{
    public delegate Order OrderProcessingDelegate(Order order);

    public class Processor
    {
        public Validator1 SomeValidator { private get; set; }
        public Binder1 SomeBinder { private get; set; }

        public DataClient DataClient { get; set; }
        public ServicesManager ServicesManager { get; private set; }


        public Processor()
        {
            DataClient = new DataClient("Security_key");
            SomeValidator = new Validator1(this);
            SomeBinder = new Binder1(this);
            ServicesManager = new ServicesManager(DataClient);
        }

        public IReadOnlyCollection<UpdateInstruction> Process(RawOrder rawOrder)
        {
            // Build list of executors for this processor
            var executors = new OrderProcessingDelegate[]
            {
                SomeValidator.BindContact,
                SomeValidator.BindSecurity,
                SomeValidator.ValidateOrder,
                SomeBinder.EvaluateCommission,
                SomeBinder.DoSomeJobWithArgument("Order")
            };
            // Firstly prepare order
            var processingOrder = new Order(rawOrder);
            // Start do single responsibility work
            foreach (var exec in executors)
            {
                processingOrder = exec(processingOrder);
            }
            // Return useful instructions
            return processingOrder.Instructions;
        }
    }


    public class Validator1 : ExecutorBase
    {
        private Processor processor;

        public Validator1(Processor processor)
        {
            this.processor = processor;
        }

        public Order BindContact(Order order)
        {
            var useSomeData = processor.DataClient.GetSomeData("Some ID1");
            return order;
        }
        public Order BindSecurity(Order order)
        {
            var useSomeData = processor.DataClient.GetSomeData("Some ID2");
            var service = processor.ServicesManager.GetService(1394);

            return order;
        }
        public Order ValidateOrder(Order order)
        {
            // Pass full order as parameter, if it needed
            var useSomeData = processor.DataClient.GetSomeData(order);

            order.AddInstruction(new UpdateInstruction("UPDATE TABLE SET COL = 1 WHERE ID = 10"));
            order.AddInstruction(new UpdateInstruction("UPDATE TABLE SET COL = 1 WHERE ID = 10"));
            order.AddInstruction(new UpdateInstruction("UPDATE TABLE SET COL = 1 WHERE ID = 10"));
            order.AddInstruction(new UpdateInstruction("UPDATE TABLE SET COL = 1 WHERE ID = 10"));
            return order;
        }
    }


    public class DataClient
    {
        public DataClient(string apiKey)
        {

        }
        public IEnumerable<object> GetSomeData(object arg)
        {
            throw new NotImplementedException();
        }
    }

    public class RawOrder
    {

    }

    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public readonly List<UpdateInstruction> Instructions = new List<UpdateInstruction>();
        private RawOrder rawOrder;

        public Order(RawOrder rawOrder)
        {
            this.rawOrder = rawOrder;
        }

        public void AddInstruction(UpdateInstruction sqlInstruction)
        {
            Instructions.Add(sqlInstruction);
        }
    }

    public abstract class ExecutorBase
    {
        public dynamic Log { get; private set; }
    }

    public class Binder1
    {
        private Processor processor;

        public Binder1(Processor processor)
        {
            this.processor = processor;
        }

        public Order EvaluateCommission(Order order)
        {
            throw new NotImplementedException();
        }
        public OrderProcessingDelegate DoSomeJobWithArgument(string usefulArgumentFromProcessor)
        {
            return order =>
            {
                // Use argument if needed
                return order;
            };
        }
    }

    public class UpdateInstruction
    {
        public string Sql { get; }
        public UpdateInstruction(string sql)
        {
            Sql = sql;
        }
    }
    public class ServicesManager
    {
        private DataClient dataClient;

        public ServicesManager(DataClient dataClient)
        {
            this.dataClient = dataClient;
            // load and cache services
        }
        public object GetService(object condit)
        {
            throw new NotImplementedException();
        }
    }
}
