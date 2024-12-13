module Program

open System
open System.IO
open System.Text.Json

type Ticket(id: int, seat: string, day: DateTime, userName: string) =
    member this.Id = id
    member this.Seat = seat
    member this.Day = day
    member this.UserName = userName

// Create
let createTicket id seat day userName = Ticket(id, seat, day, userName)

// Write
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

// Read
let readTicketsFromFile () =
    let filePath = "tickets.json"

    if File.Exists(filePath) then
        let json = File.ReadAllText(filePath)
        JsonSerializer.Deserialize<Ticket list>(json)
    else
        []

// Create example
let newTicket = createTicket 1 "A1" (DateTime.Now) "John Doe"
appendToTicketsFile newTicket

// Read example
let tickets = readTicketsFromFile ()
printfn "Tickets: %A" tickets
