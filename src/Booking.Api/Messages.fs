﻿namespace Booking.Api

open System

[<CLIMutable>]
type MakeReservation = {
    Date : DateTime
    Name : string
    Email : string
    Quantity : int }

module Envelope =
    [<CLIMutable>]
    type Envelope<'T> = {
        Id : Guid
        Created : DateTimeOffset
        Item : 'T }

    let Envelop id created item = {
        Id = id
        Created = created
        Item = item }

    let EnvelopWithDefaults item = 
        Envelop (Guid.NewGuid()) DateTimeOffset.Now item

[<CLIMutable>]
type Reservation = {
    Date : DateTime
    Name : string
    Email : string
    Quantity : int }
