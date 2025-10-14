$(document).ready(function () {
    $("#airportInput").keyup(function () {
        var keyword = $(this).val();
        if (keyword.length > 0) {
            $("#airportResults").empty();
            var queryString = "keywordSearch=" + encodeURIComponent(keyword);
            $.ajax({
                url: '/Encryptedquery',
                type: 'POST',
                data: { plainText: queryString },
                success: function (encryptedQuery) {
                    $.ajax({
                        url: '/Timkiemsanbay/' + encodeURIComponent(encryptedQuery),
                        type: 'GET',
                        success: function (data) {
                            $("#airportResults").empty();
                            if (data.length > 0) {
                                $.each(data, function (i, item) {
                                    $("#airportResults").append(
                                        "<li onclick=\"checkRoute(this, '" + item.AirportCode + "')\">" + item.CityName + "</li>"
                                    );
                                });
                            } else {
                                $("#airportResults").append("<li>Không tìm thấy kết quả</li>");
                            }
                        }
                    });
                }
            });
        } else {
            $("#airportResults").empty();
            $("#airportResults").append("<li onclick=\"checkRoute(this, 'HAN')\">Hà Nội (HAN)</li>");
            $("#airportResults").append("<li onclick=\"checkRoute(this,'SGN')\">Hồ Chí Minh (SGN)</li>");
            $("#airportResults").append("<li onclick=\"checkRoute(this, 'DAD')\">Đà Nẵng (DAD)</li>");
            $("#airportResults").append("<li onclick=checkRoute(this,'DIN')\">Điện Biên Phủ (DIN)</li>");
            $("#airportResults").append("<li onclick=\"checkRoute(this,'HPH')\">Hải Phòng (HPH)</li>");
            $("#airportResults").append("<li onclick=\"checkRoute(this,'THD')\">Thanh Hóa (THD)</li>");
            $("#airportResults").append("<li onclick=\"checkRoute(this,'VII')\">Vinh (VII)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'VDH')\"> Quảng Bình(VDH)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'VCA')\"> Cần Thơ(VCA)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'VKG')\"> Kiên Giang(VKG)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'VCL')\"> Quảng Nam(VCL)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'HUI')\"> Huế(HUI)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'PXU')\"> PleiKu(PXU)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'TBB')\"> Phú Yên(TBB)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'BMV')\"> Ban Mê Thuột(BMV)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'CXR')\"> Nha Trang(CXR)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'UIH')\"> Qui Nhơn(UIH)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'DLI')\"> Đà Lạt(DLI)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'CAH')\"> Cà Mau(CAH)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'PQC')\"> Phú Quốc(PQC)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'VCS')\"> Côn Đảo(VCS)</li>");
            $("#airportResults").append("<li onclick =\"checkRoute(this,'VDO')\"> Vân Đồn(VDO)</li>");
        }
    });
    $('#btn-search').on('click', function (e) {
        if (checkValidateInputSearch()) {
            var $btn = $(this);
            $btn.prop('disabled', true);
            var originalText = $btn.html();
            $btn.css("display", "flex");
            $btn.css("flex-flow", "row");
            $btn.css("justify-content", "center");
            $btn.css("align-items", "center");
            $btn.html('<img src="/Content/img/loading.gif" style="width:30px;height:30px;vertical-align:middle;margin-right:5px;margin-bottom: 0;" /> Tìm kiếm...');

            // Chuẩn bị data gửi đi
            var data = {
                departure: $("#departureAirport").val().trim(),
                arrival: $("#arrivalAirport").val().trim(),
                departureDate: $("#departureDate").val().trim(),
                returnDate: $("#returnDate").val().trim() || null,
                roundType: parseInt($("#roundType").val().trim()),
                countAdt: parseInt($("#nguoiLon").val().trim()),
                countChd: parseInt($("#treEm").val().trim()),
                countInf: parseInt($("#emBe").val().trim())
            };

            var startTime = new Date().getTime();

            $.ajax({
                url: '/Cheksearchflightinput',
                type: 'POST',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (response) {
                    if (response && response.dataResult) {
                        var elapsed = new Date().getTime() - startTime;
                        var remaining = 3000 - elapsed; 

                        if (remaining < 0) remaining = 0;

                        setTimeout(function () {
                            // Redirect sang trang kết quả, kèm sessionId
                            window.location.href = '/Ketquatimkiem?data=' + response.dataResult;
                        }, remaining);
                    } else {
                        alert("Không nhận được sessionId từ server!");
                        $btn.prop('disabled', false).html(originalText);
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Lỗi: ", error);
                    alert("Đã có lỗi xảy ra, vui lòng thử lại!");
                    $btn.prop('disabled', false).html(originalText);
                }
            });
        }
    });
});
function checkValidateInputSearch() {
    let departure = $("#departureAirport").val().trim();
    let arrival = $("#arrivalAirport").val().trim();
    let departureDate = $("#departureDate").val().trim();
    let returnDate = $("#returnDate").val().trim();
    let roundType = parseInt($("#roundType").val().trim());
    let countAdt = parseInt($("#nguoiLon").val().trim());
    let countChd = parseInt($("#treEm").val().trim());
    let countInf = parseInt($("#emBe").val().trim());
    if (departure === "") {
        $("#isErooDepartureAirport").css("display", "block");
        $('#isErooDepartureAirport')
            .text("Bạn chưa chọn điểm đi");
        return false;
    }
    else {
        $("#isErooDepartureAirport").css("display", "none");
    }
    if (arrival === "") {
        $("#isErooArrivalAirport").css("display", "block");
        $('#isErooArrivalAirport')
            .text("Bạn chưa chọn điểm đến");
        return false;
    }
    else {
        $("#isErooArrivalAirport").css("display", "none");
    }
    if (departure === arrival) {
        $("#isErooArrivalAirport").css("display", "block");
        $('#isErooArrivalAirport')
            .text("Điểm đến không được trùng điểm đi");
        return false;
    }
    else {
        $("#isErooArrivalAirport").css("display", "none");
    }
    if (departureDate === "") {
        $("#isErooDepartureDate").css("display", "block");
        $('#isErooDepartureDate')
            .text("Bạn chưa chọn ngày đi");
        return false;
    }
    else
    {
        if (isValidDate(departureDate)) {
            $("#isErooDepartureDate").css("display", "none");
        }
        else {
            $("#isErooDepartureDate").css("display", "block");
            $('#isErooDepartureDate')
                .text("Không đúng định dạng dd/mm/yyyy");
            return false;
        }
    }
    if (roundType == 1) {
        if (returnDate === "") {
            $("#isErooReturnDate").css("display", "block");
            $('#isErooReturnDate')
                .text("Bạn chưa chọn ngày về");
            return false;
        }
        else {
            if (isValidDate(returnDate)) {
                if (isValidDateRange(departureDate, returnDate)) {
                    $("#isErooReturnDate").css("display", "none");
                }
                else {
                    $("#isErooReturnDate").css("display", "block");
                    $('#isErooReturnDate')
                        .text("Ngày về phải lớn hơn ngày đi");
                    return false;
                }
            }
            else {
                $("#isErooReturnDate").css("display", "block");
                $('#isErooReturnDate')
                    .text("Không đúng định dạng dd/mm/yyyy");
                return false;
            }
        }
    }
    let totalPax = countAdt + countChd + countInf;
    if (totalPax > 9) {
        $("#isErooNumPax").css("display", "block");
        $('#isErooNumPax')
            .text("Hành khách không quá 9 người");
        return false;
    }
    else {
        if (totalPax == 0) {
            $("#isErooNumPax").css("display", "block");
            $('#isErooNumPax')
                .text("Số lượng hành khách phải lớn hơn 0");
            return false;
        }
        else {
            if (countInf > countAdt) {
                $("#isErooNumPax").css("display", "block");
                $('#isErooNumPax')
                    .text("Số lượng em bé không được lớn hơn người lớn");
                return false;
            }
            else {
                $("#isErooNumPax").css("display", "none");
            }
        }
    }
    return true;
}
function checkRoute(elementClick, code) {
    modal2.style.display = 'none'
    const elements = document.querySelectorAll('.point-route')
    elements.forEach(el => {
        if (
            isArrCheck[0] == el.getAttribute('data-position-box') &&
            isArrCheck[1] == el.getAttribute('data-position-item')
        ) {
            el.textContent = elementClick.textContent
            el.nextElementSibling.value = code
        }
    })
    isArrCheck = []
}