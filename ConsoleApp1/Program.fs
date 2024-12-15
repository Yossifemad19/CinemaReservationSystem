open System
open System.Windows.Forms
open System.Drawing
open System.IO

open Data
open Ticket

[<EntryPoint>]
let main argv=
    let cinemaDataPath="CinemaData.json"

    if not (File.Exists(cinemaDataPath)) then
        let data = generateWeeklyBookings 4 4
        saveDataToJson data cinemaDataPath


    let form=new Form(Text="Cinema Reservation System",Width=1000,Height=550)

    let panal = new Panel(Width=800,Height=550,AccessibleName="seatScrean",BackColor=Color.LightGray)

    

    let userNameInput=new TextBox(AccessibleName="UserNameInput",Width=100,
                                    Height=80,Location=new Point(850,50),Text="userName")

    let daysDropDown=new ComboBox(AccessibleName="daysDropDown",Width=100,
                                    Height=80,Location=new Point(850,250))

    daysDropDown.DataSource <- [|"Saturday"; "Sunday"; "Monday"; "Tuesday"; "Wednesday"; "Thursday"; "Friday"|]
    daysDropDown.DropDownStyle <- ComboBoxStyle.DropDownList


    let partiesDropDown=new ComboBox(AccessibleName="partiesDropDown",Width=100,
                                    Height=80,Location=new Point(850,300))

    partiesDropDown.DataSource <-[|"12 am"; "3 pm"; "6 pm"; "9 pm"|]
    partiesDropDown.DropDownStyle <- ComboBoxStyle.DropDownList


    let Display_Seats(panal:Panel,cinemaData:CinemaData)=
            panal.Controls.Clear()
            let mutable x=0
            let mutable y=0
            for i in 0..cinemaData.Rows-1 do
                for j in 0..cinemaData.Cols-1 do

                    let groupBox =new GroupBox(
                        Text = $"seat {i+1}.{j+1}",
                        Width=200,
                        Height=125,
                        Location=new Point(x,y)
                    )
                    let infoButton = new Button(
                        Text="info",
                        Width=100,
                        Height=30,
                        Location=new Point(50,85),
                        BackColor=Color.Black,
                        ForeColor=Color.Wheat
                        )
                    infoButton.Click.Add(fun _ ->
                        let ticket = readSpecificTicket cinemaData.Day cinemaData.Party (i,j)
                        match ticket with 
                        | Some(ticket) ->
                            MessageBox.Show($"ticket id ->\t{ticket.Id} \n user name ->\t{ticket.UserName}") |>ignore
                        | None ->
                            // MessageBox.Show($"none ticket") |>ignore
                            printfn "No Ticket"
                        )
                    if cinemaData.Seats[i].[j]=false then
                        let bookButton = new Button(
                            Text="Book",
                            Width=100,
                            Height=30,
                            Location=new Point(50,85),
                            BackColor=Color.Green,
                            ForeColor=Color.Wheat
                        )
                        bookButton.Click.Add(fun _ ->
                            let userName= userNameInput.Text
                            if userName="userName" || userName="" then
                                MessageBox.Show("Please provide a user name") |> ignore
                            else
                                cinemaData.UpdateSeat(i,j)
                                updateSeatsJson cinemaData.Day cinemaData.Party cinemaDataPath cinemaData
                                let ticket = createTicket cinemaData.Day cinemaData.Party (i,j) userName
                                appendToTicketsFile ticket
                                groupBox.Controls.Clear()
                                groupBox.Controls.Add(infoButton)
                        )
                        groupBox.Controls.Add(bookButton)
                    else
                        groupBox.Controls.Add(infoButton)

                    panal.Controls.Add(groupBox)
                    x <- x+200
                
                x <- 0
                y <- y+125


    let applyButton = new Button(AccessibleName="applyButton",Width=100,Height=30,
                                Location=new Point(850,400),Text="Apply",BackColor=Color.Gray)
    applyButton.Click.Add(fun _ ->
            let day = daysDropDown.SelectedValue.ToString()
            let party = partiesDropDown.SelectedValue.ToString()
            let partyData =readCinemaData day party cinemaDataPath
            match partyData with
            | Some data ->
                // MessageBox.Show($"{data.Seats}")
                Display_Seats(panal, data)
            | None ->
                MessageBox.Show("No matching data found for the selected day and party.") |>ignore
    )


    form.Controls.Add(panal)
    form.Controls.Add(userNameInput)

    form.Controls.Add(daysDropDown)
    form.Controls.Add(partiesDropDown)
    form.Controls.Add(applyButton)


    Application.Run(form)

    0