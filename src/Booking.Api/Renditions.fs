namespace Booking.Api

open System

[<CLIMutable>] // so normal .NET serializers can serialize and deserialize this record
type MakeReservationRendition = {
    Date : string
    Name : string
    Email : string
    Quantity : int }
