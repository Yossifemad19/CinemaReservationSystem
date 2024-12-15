module Data
open System.IO
open System.Text.Json
open System.Text.Json.Serialization

type CinemaDataDTO = {
    Day: string
    Party: string
    Seats: bool list list
}


type CinemaData(day: string, party: string, rows: int, cols: int) =
    let mutable seats = Array2D.create rows cols false
    let day = day
    let party = party

    member this.Day with get() = day
    member this.Party with get() = party
    member this.Rows with get() = rows
    member this.Cols with get() = cols

    member this.Seats
        with get() = 
            // Convert Array2D to List of Lists for serialization
            [ for i in 0 .. rows - 1 -> [ for j in 0 .. cols - 1 -> seats.[i, j] ] ]

    member this.UpdateSeat(row: int, col: int) =
        if row >= 0 && row < rows && col >= 0 && col < cols then
            seats.[row, col] <- true
        else
            failwith "Invalid row or column index"

    member this.ToDto()=
                { Day = day; Party = party; Seats = this.Seats }

    static member FromDTO(dto: CinemaDataDTO) =
        let rows = dto.Seats.Length
        let cols = if rows > 0 then dto.Seats.[0].Length else 0
        let instance = CinemaData(dto.Day, dto.Party, rows, cols)
        for i in 0 .. rows - 1 do
            for j in 0 .. cols - 1 do
                if dto.Seats.[i].[j] = true then
                    instance.UpdateSeat(i, j) 
        instance
    
    // member this.

let generateWeeklyBookings rows cols =
    let daysOfWeek = ["Saturday"; "Sunday"; "Monday"; "Tuesday"; "Wednesday"; "Thursday"; "Friday"]
    let partiesTimes = ["12 am"; "3 pm"; "6 pm"; "9 pm"]

    [ for day in daysOfWeek do
        for party in partiesTimes do
            yield CinemaData(day, party, rows, cols) ]

let saveDataToJson (cinemaData:CinemaData list) filename =
    let cinemaDto = cinemaData |>List.map(fun d -> d.ToDto())
    let options = JsonSerializerOptions(WriteIndented = true)
    let json = JsonSerializer.Serialize(cinemaDto, options)
    File.WriteAllText(filename, json)
    printfn "Data saved to %s" filename


let readCinemaData (day: string) (party: string) (filename: string):CinemaData option  =
 
    if File.Exists(filename) then
        let json = File.ReadAllText(filename)
        let records = JsonSerializer.Deserialize<CinemaDataDTO  list>(json)
        
        records |> List.tryFind (fun record -> 
            record.Day.Equals(day, System.StringComparison.OrdinalIgnoreCase) && 
            record.Party.Equals(party, System.StringComparison.OrdinalIgnoreCase)) 
            |> Option.map CinemaData.FromDTO
    else
        None

        



let updateSeatsJson (day: string) (party: string) (filename: string) (updatedRecord: CinemaData) =
    try
        let json = File.ReadAllText(filename)
        let records = JsonSerializer.Deserialize<CinemaDataDTO list>(json)

        if List.isEmpty records then
            printfn "No records found in the file."
        else
            let updatedRecords =
                records 
                |> List.map (fun record ->
                    if record.Day.Equals(day, System.StringComparison.OrdinalIgnoreCase) &&
                       record.Party.Equals(party, System.StringComparison.OrdinalIgnoreCase) then
                        let updatedData = updatedRecord.ToDto() 
                        updatedData
                    else
                        record
                )

            let updatedJson = JsonSerializer.Serialize(updatedRecords, JsonSerializerOptions(WriteIndented = true))
            File.WriteAllText(filename, updatedJson)
            printfn "Seats updated successfully."

    with
    | :? IOException as e -> 
        printfn "File Error: %s" e.Message
    | :? JsonException as e -> 
        printfn "JSON Parsing Error: %s" e.Message
    | ex -> 
        printfn "Unexpected Error: %s" ex.Message


