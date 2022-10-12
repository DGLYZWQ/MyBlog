$.ajax({

    url: "../../../Backend/Layout/GetUsersInfo",
    type: "get",
    dataType: "json",
    success: function (data) {
        $("#usersName").text(data.NickName);
        var url = "../../../upload/Users/" + data.SmallImage;
        $("#usersPhoto").attr("src", url);

    }


});