

//These functions mainly operate as means to collect the user's input data and send them to the assigned controller methods. The variable 'counter' exists to make sure that the user may add at most 10 "Add new phone 
//number" input fields when creating/editing a contact. A couple of these functions serve as simple DOM manipulation purposes(to fetch/hide/clear modal "TextBox"). "ValidateContact" function can be found inside 
//the "_TextBox" partial view;

var counter = 0;

function clearTextBox() {
    $('#Name').val("");
    $('#Email').val("");
    $('#Address').val("");
    $('#phoneNumbersContainer').html('');
    $('#btnUpdate').hide();
    $('#btnContactAdd').show();
    $('#Name').css('border-color', 'lightgrey');
    $('#Email').css('border-color', 'lightgrey');
    $('#Address').css('border-color', 'lightgrey');
    counter = 0;
}


$('#addPhone').click(function () {

    if (counter < 10) {
        counter += 1;
        $('#phoneNumbersContainer').append('<div class="form-group" id="form' + counter + '"><button type="button" class="close pull-left" onclick="closeInput(' + counter + ')">×</button><label for="phone' + counter + '">NEW Phone Number</label><input type="text" class="form-control phoneNumberText" id="phone' + counter + '" value=""></div>');
    }
    else {
        alert('Cannot add more than 10 phone numbers at once.');
        return false;
    }
});

function closeInput(Id) {
    $('#form' + Id + '').remove();
    counter--;
}

function addContact() {
    var res = validateContact();

    if (res == false) {
        return false;
    }

    let Name = $('#Name').val();
    let Email = $('#Email').val();
    let Address = $('#Address').val();
    let PhoneNumbers = [];

    $.each($(".phoneNumberText"), function (key, item) {
        if (item.value.trim() != "") {
            PhoneNumbers.push(item.value);
        }
    });

    $.ajax({
        url: "/Home/CreateContact",
        data: JSON.stringify({ Name: Name, Email: Email, Address: Address, PhoneNumbers: PhoneNumbers }),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myModal').modal('hide');
            location.reload();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getbyID(Id) {
    $('#Name').css('border-color', 'lightgrey');
    $('#Email').css('border-color', 'lightgrey');
    $('#Address').css('border-color', 'lightgrey');
    counter = 0;

    $.ajax({
        url: "/Home/getbyID/",
        data: JSON.stringify({ Id: Id }),
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {

            $('#Name').val(result.Name);
            $('#Email').val(result.Email);
            $('#Address').val(result.Address);
            $('#phoneNumbersContainer').html('');

            if (result.PhoneNumbers != null) {
                $.each(result.PhoneNumbers, function (key, item) {
                    $('#phoneNumbersContainer').append('<div class="form-group" id="form' + counter + '"><button type="button" class="close pull-left" onclick="closeInput(' + counter + ')">×</button><label for="phone' + counter + '">Phone Number</label><input type="text" pattern="[0-9]" class="form-control phoneNumberText" id="phone' + counter + '" value="' + item.Number + '"></div>');
                    counter++
                });
            }

            $('#UpdateButton').append('<button type="button" class="btn btn-primary" id="btnUpdate" style="display:none;" onclick="UpdateContact(\'' + result.Id + '\');">Update</button>')
            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnContactAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function UpdateContact(Id) {
    var res = validateContact(Id);

    if (res == false) {
        return false;
    }

    var Name = $('#Name').val();
    var Email = $('#Email').val();
    var Address = $('#Address').val();
    var PhoneNumbers = [];

    $.each($(".phoneNumberText"), function (key, item) {
        if (item.value.trim() != "") {
            PhoneNumbers.push(item.value);
        }
    });


    $.ajax({
        url: "/Home/UpdateContact",
        data: JSON.stringify({ Id: Id, Name: Name, Email: Email, Address: Address, PhoneNumbers: PhoneNumbers }),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            location.reload();
            $('#myModal').modal('hide');
            $('#Name').val("");
            $('#Email').val("");
            $('#Address').val("");
            $('#phoneNumbersContainer').html('');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function DeleteContact(Id) {
    var ans = confirm("Are you sure? All of the contact's data including phone numbers will be lost.");

    if (ans) {
        $.ajax({
            url: "/Home/DeleteContact/" + Id,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                location.reload();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}


