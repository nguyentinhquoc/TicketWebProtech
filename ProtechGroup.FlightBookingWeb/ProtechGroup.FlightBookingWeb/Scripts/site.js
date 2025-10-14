$(document).ready(function () {
    // Ẩn danh sách giá rẻ khi load trang
    $('#tickets-list-items-gia-re').hide()
    $('#tickets-list-items-don-moi').show()
    // Khi click vào radio "Đơn mới"
    $('#don-moi').on('change', function () {
        if ($(this).is(':checked')) {
            $('#tickets-list-items-don-moi').slideDown('slow')
            $('#tickets-list-items-gia-re').slideUp('slow')
        }
    })

    // Khi click vào radio "Giá rẻ"
    $('#gia-re').on('change', function () {
        if ($(this).is(':checked')) {
            $('#tickets-list-items-gia-re').slideDown('slow')
            $('#tickets-list-items-don-moi').slideUp('slow')
        }
    })
})