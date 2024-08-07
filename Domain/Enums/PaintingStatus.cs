namespace Domain.Enums;

public enum PaintingStatus
{
    Draft, //The submission has been started but not yet submitted. The participant is still working on it.
    Submitted, //The submission has been completed and officially submitted for review.
    Delete, //The submission has delete by the competitor
    Accepted, //The submission has been accepted by the staff.
    Rejected, //The submission has been rejected by the staff.
    Pass, //The painting is pass
    NotPass, //The painting is not pass

    // For Final Round

    FinalRound, // Status of painting in round 2
    HasPrizes // status when painting has prize
}