
$(document).ready(function () {
    $('#SelectedCategoryId').change(function () {
        var categoryId = $(this).val();
        if (categoryId) {
            $.ajax({
                url: '/Home/GetSubcategories',
                type: 'GET',
                data: { categoryId: categoryId },
                success: function (data) {
                    $('#SubcategoryId').empty();
                    $('#SubcategoryId').append($('<option>').text('Select Subcategory').attr('value', ''));
                    $.each(data, function (index, subcategory) {
                        $('#SubcategoryId').append($('<option>').text(subcategory.subCategoryName).attr('value', subcategory.subCategoryId));
                    });
                }
            });
        } else {
            $('#SubcategoryId').empty();
            $('#SubcategoryId').append($('<option>').text('Select Subcategory').attr('value', ''));
        }
    });
});







$(document).ready(function () {
    $('.subcategory-link').click(function (e) {
        e.preventDefault();
        var categoryId = $(this).data('category-id');
        var subcategoryId = $(this).data('subcategory-id');
        if (categoryId && subcategoryId) {
          
            $('<input>').attr({
                type: 'hidden',
                name: 'category',
                value: categoryId
            }).appendTo('form.search-form');
            $('<input>').attr({
                type: 'hidden',
                name: 'subcategory',
                value: subcategoryId
            }).appendTo('form.search-form');
           
            $('form.search-form').submit();
        }
    });
});


$(document).ready(function () {
    $('.list-group-item').hover(function () {
        $(this).find('.subcategories').show();
    }, function () {
        $(this).find('.subcategories').hide();
    });
});



$(document).ready(function () {
    $('.category-link').click(function (e) {
        e.preventDefault();
        var categoryId = $(this).data('category-id');
        $('#searchForm').append('<input type="hidden" name="category" value="' + categoryId + '">');
        $('#searchForm').submit();
    });
});