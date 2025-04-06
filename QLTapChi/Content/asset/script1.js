// JavaScript để hiển thị/ẩn nội dung tóm tắt cho từng bài báo
document.querySelectorAll('.tomTatButton').forEach(button => {
    button.addEventListener('click', function(event) {
        event.preventDefault(); // Ngăn chặn hành động mặc định của thẻ <a>
        const content = this.closest('.mb-4').querySelector('.tomTatContent');
        if (content.style.display === 'none') {
            content.style.display = 'block';
        } else {
            content.style.display = 'none';
        }
    });
});