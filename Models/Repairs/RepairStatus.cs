namespace cinnamon.api.Models
{
    public enum RepairStatus
    {
        Accepted = 100,
        Assigned = 200,
        AwaitingApproval = 300,
        CustomerApproved = 400,
        CustomerRejected = 500,
        AwaitingParts= 600,
        Ready = 700,
        AwaitingCustomer = 800,
        Closed = 900,
        UnRepairable = 1000
    }
}