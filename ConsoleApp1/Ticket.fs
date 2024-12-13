module Ticket

open System
open System.IO
open System.Text.Json

type Ticket( day: string,party:string,seat:int*int, userName: string) =

    member this.Id = Guid.NewGuid()
    member this.Seat = seat
    member this.Day = day
    member this.Party = party
    member this.UserName = userName

let createTicket day party seat userName = Ticket(day,party,seat,userName)


let appendToTicketsFile (ticket: Ticket) =
    let filePath = "tickets.json"

    let tickets =
        if File.Exists(filePath) then
            let json = File.ReadAllText(filePath)
            JsonSerializer.Deserialize<Ticket list>(json)
        else
            []
    let updatedTickets = ticket :: tickets

    let updatedJson =
        JsonSerializer.Serialize(updatedTickets, JsonSerializerOptions(WriteIndented = true))

    File.WriteAllText(filePath, updatedJson)

let readSpecificTicket (day: string) (party: string) (seat: int*int) =
    let filePath = "tickets.json"

    if File.Exists(filePath) then
        let json = File.ReadAllText(filePath)
        let tickets = JsonSerializer.Deserialize<Ticket list>(json)

        // Filter the tickets based on the parameters
        tickets
        |> List.tryFind (fun ticket ->
            ticket.Day = day && ticket.Party = party && ticket.Seat = seat)
    else
        None