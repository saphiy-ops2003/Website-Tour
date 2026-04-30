export default function Footer() {
  return (
    <footer className="bg-slate-900 text-white py-16">
      <div className="max-w-7xl mx-auto px-6 font-sans">
        <div className="flex flex-col md:flex-row justify-between items-start gap-12 mb-16">
          <div className="max-w-xs">
            <div className="text-3xl font-black tracking-tighter text-white mb-6">VietExplore</div>
            <p className="text-sm text-slate-400 leading-relaxed">
              VietExplore là đơn vị lữ hành hàng đầu, kiến tạo những hành trình khám phá Việt Nam đậm chất bản địa và đẳng cấp quốc tế.
            </p>
          </div>
          <div className="grid grid-cols-2 md:grid-cols-3 gap-16 text-sm">
            <div>
              <h4 className="font-bold text-white mb-6 uppercase text-xs tracking-widest">Dịch vụ</h4>
              <ul className="space-y-4 text-slate-400">
                <li><a href="#" className="hover:text-accent transition-colors">Tour nội địa</a></li>
                <li><a href="#" className="hover:text-accent transition-colors">Tour quốc tế</a></li>
                <li><a href="#" className="hover:text-accent transition-colors">Đặt khách sạn</a></li>
              </ul>
            </div>
            <div>
              <h4 className="font-bold text-white mb-6 uppercase text-xs tracking-widest">Hỗ trợ</h4>
              <ul className="space-y-4 text-slate-400">
                <li><a href="#" className="hover:text-accent transition-colors">Câu hỏi thường gặp</a></li>
                <li><a href="#" className="hover:text-accent transition-colors">Liên hệ</a></li>
                <li><a href="#" className="hover:text-accent transition-colors">Góp ý dịch vụ</a></li>
              </ul>
            </div>
            <div>
              <h4 className="font-bold text-white mb-6 uppercase text-xs tracking-widest">Pháp lý</h4>
              <ul className="space-y-4 text-slate-400">
                <li><a href="#" className="hover:text-accent transition-colors">Chính sách bảo mật</a></li>
                <li><a href="#" className="hover:text-accent transition-colors">Điều khoản sử dụng</a></li>
              </ul>
            </div>
          </div>
        </div>
        <div className="flex flex-col md:flex-row justify-between items-center gap-8 pt-10 border-t border-slate-800 text-slate-500 text-sm">
          <div>© 2024 VietExplore. Cảm hứng du lịch Việt Nam.</div>
          <div className="flex gap-6">
            {/* Social Icons Placeholder */}
          </div>
        </div>
      </div>
    </footer>
  );
}
