# Sửa DB:

# Validate:
- Validate Painting có AwardId và RoundId không cùng trong 1 cuộc thi
- Bắt validate add RoundTOpic ( Add Trùng)
- Bắt validate add PaintingCollection (Add Trùng)
- Chưa bắt validate date contest
- Tên topic khônng trùng nhau
- Validate tuổi khi đăng ký vào contest

# Sửa: 
- Sửa viewmodel resource lấy các bảng liên quan
- xem các cuộc thi
- xem chi tiết cuộc thi
- xem các bộ sưu tập
- xem các thí sinh có giải 5 năm gần đây
- Nếu xóa Contest thì status của những bảng liên quan sẽ đổi luôn
- Sửa Output cho GetContestById
- (Xóa Contest sẽ đổi status của những bảng link Contest(Level, Round, Topic, Sponsor

- Lấy tranh lấy luôn award 
- Chuyển Stauts của report
- Đổi path Get Paintg By Collection(Controller)
- Lấy ra roundid topicid từ roundtopicid trong painting (paiting view model) ( Lấy Round)
- List ra các Account(code,name)
- Kiếm accountbycode
- Viết Generate code account, tranh ghi staff duyệt

  
# Xong :white_check_mark:
- Get Painting By AccountId :white_check_mark:
- Tạo Round add cho toàn bộ level (thay levelid = contestid) với bỏ listtopic :white_check_mark:
- Add nhiều topicround :white_check_mark:
- get alltopic (không phân trang) :white_check_mark:
