import { motion } from 'motion/react';
import { Bolt, ArrowRight, Plane, Hotel, Utensils, Copy, Star } from 'lucide-react';

export default function Deals({ setView }: { setView: (v: string) => void }) {
  return (
    <div className="bg-white min-h-screen font-sans pb-24">
      {/* Hero with Flash Sale */}
      <div className="max-w-7xl mx-auto px-6 py-12">
        <section className="relative rounded-3xl overflow-hidden h-[450px] flex items-center shadow-2xl">
          <img 
            src="https://lh3.googleusercontent.com/aida-public/AB6AXuBswWWkqlCsiQoK3ACgn27fFNoHsqRUT827n4V4EOK8q4g_dPXWlneWk4LyULS4FZYcphEeha3hYX5zuqVtp-eYB2dx3zJ5wjbn4GqQJsVAKhlgUz39XElR0n7Qb8P4BXx0vKmLjumFtRrssKsLBb3L4vlRi4Jl9fyRJZ2JtWD9n3ZAz2lX9VR5sGxwV6B2cjWDpBv65Wg_ulACHUuuTTSE1rl2X_hxEjmQSZmm1DzIUW9Dpf6GyKdbhwPE6UB97r-JEEOrParMSw" 
            alt="Ha Long Sunset" 
            className="absolute inset-0 w-full h-full object-cover"
          />
          <div className="absolute inset-0 bg-gradient-to-r from-black/80 via-black/40 to-transparent"></div>
          <div className="relative z-10 p-16 max-w-3xl text-white">
            <div className="inline-flex items-center gap-2 bg-orange-500 text-white px-4 py-2 rounded-full mb-6">
              <Bolt className="w-5 h-5 fill-white" />
              <span className="font-bold text-xs uppercase tracking-widest">Flash Sale: Ưu đãi giới hạn</span>
            </div>
            <h1 className="text-5xl font-bold mb-4 leading-tight">Khám Phá Di Sản Hạ Long – Ưu Đãi 40%</h1>
            <p className="text-xl mb-10 text-slate-200">Dành riêng cho 50 khách hàng đầu tiên đặt tour du thuyền 5 sao trong tuần này.</p>
            
            <div className="flex items-center gap-8">
              <div className="flex gap-3">
                 {[ {v:'02',l:'Ngày'}, {v:'14',l:'Giờ'}, {v:'56',l:'Phút'}].map((t, idx) => (
                   <div key={idx} className="bg-slate-900 w-16 h-16 rounded-xl flex flex-col items-center justify-center border border-white/10">
                     <div className="text-xl font-bold">{t.v}</div>
                     <div className="text-[8px] uppercase opacity-60 font-bold">{t.l}</div>
                   </div>
                 ))}
              </div>
              <button className="bg-accent hover:bg-accent-dark text-white px-8 py-4 rounded-xl font-bold shadow-xl transition-all flex items-center gap-2">
                Nhận Ưu Đãi Ngay <ArrowRight className="w-5 h-5" />
              </button>
            </div>
          </div>
        </section>

        {/* Coupons Grid */}
        <section className="mt-24">
          <div className="flex justify-between items-end mb-12">
            <div>
              <h2 className="text-4xl font-bold text-primary-dark">Mã Giảm Giá Độc Quyền</h2>
              <p className="text-slate-500 mt-2 text-lg">Lưu ngay mã giảm giá để nhận ưu đãi cho chuyến đi tiếp theo.</p>
            </div>
            <button className="text-primary font-bold flex items-center gap-1 hover:underline">
              Xem tất cả mã <ArrowRight className="w-4 h-4 ml-1" />
            </button>
          </div>
          
          <div className="grid grid-cols-1 md:grid-cols-3 gap-10">
             {[
               { icon: <Plane className="w-8 h-8" />, title: 'Giảm 200k', desc: 'Áp dụng cho mọi vé máy bay khứ hồi nội địa.', code: 'FLYVN200', tag: 'MÁY BAY', color: 'blue' },
               { icon: <Hotel className="w-8 h-8" />, title: 'Giảm 15%', desc: 'Tối đa 500k khi đặt phòng khách sạn từ 2 đêm trở lên.', code: 'STAYHAPPY', tag: 'KHÁCH SẠN', color: 'orange' },
               { icon: <Utensils className="w-8 h-8" />, title: 'Tặng Buffet', desc: 'Miễn phí bữa tối cao cấp cho tour trọn gói Đà Nẵng.', code: 'FREEFOOD', tag: 'TOUR', color: 'green' }
             ].map((c, i) => (
               <div key={i} className="bg-white border border-slate-200 rounded-3xl p-8 relative flex flex-col h-full shadow-sm hover:shadow-xl transition-all group">
                  <div className="flex justify-between items-start mb-8">
                    <div className="w-14 h-14 bg-slate-50 text-primary rounded-2xl flex items-center justify-center group-hover:bg-primary group-hover:text-white transition-all">
                      {c.icon}
                    </div>
                    <span className="bg-slate-100 px-3 py-1 rounded-lg text-[10px] font-bold uppercase tracking-widest text-slate-500">{c.tag}</span>
                  </div>
                  <div className="mb-8">
                     <div className="text-2xl font-bold text-slate-800 mb-1">{c.title}</div>
                     <p className="text-slate-500 text-sm">{c.desc}</p>
                  </div>
                  <div className="mt-auto pt-6 border-t border-dashed border-slate-200 flex items-center justify-between gap-4">
                    <div className="bg-slate-50 px-4 py-3 rounded-xl border border-slate-100 flex-1">
                      <code className="font-bold text-slate-800 tracking-widest">{c.code}</code>
                    </div>
                    <button className="bg-primary text-white p-3 rounded-xl shadow-lg active:scale-95 transition-all">
                      <Copy className="w-4 h-4" />
                    </button>
                  </div>
               </div>
             ))}
          </div>
        </section>

        {/* Loyalty Section */}
         <section className="mt-24 bg-primary-dark rounded-[3rem] p-12 md:p-20 relative overflow-hidden text-white">
           <div className="relative z-10 flex flex-col md:flex-row items-center gap-12">
             <div className="flex-1">
                <div className="inline-flex items-center gap-2 bg-blue-400/20 px-4 py-2 rounded-full mb-8">
                  <Star className="w-4 h-4 fill-white" />
                  <span className="text-xs font-bold uppercase tracking-widest">Thành viên ưu tiên</span>
                </div>
                <h2 className="text-5xl font-bold mb-6">Gia nhập VietExplore Rewards</h2>
                <p className="text-xl text-blue-100 mb-10 leading-relaxed max-w-lg">Nhận ngay 100 điểm thưởng khi đăng ký. Hưởng đặc quyền giảm giá lên đến 10%.</p>
                <div className="flex gap-4">
                  <button className="bg-white text-primary px-10 py-5 rounded-2xl font-bold text-lg">Đăng ký miễn phí</button>
                  <button className="border border-white/20 px-10 py-5 rounded-2xl font-bold text-lg hover:bg-white/10">Tìm hiểu đặc quyền</button>
                </div>
             </div>
             <div className="hidden lg:block w-[400px]">
                <div className="bg-slate-50 rounded-[2.5rem] p-8 text-slate-800 shadow-2xl rotate-3">
                   <div className="flex justify-between items-start mb-12">
                      <div className="w-12 h-12 bg-primary/10 rounded-xl flex items-center justify-center">
                         <Star className="text-primary fill-primary" />
                      </div>
                      <div className="text-right">
                         <div className="text-[10px] text-slate-400 font-bold uppercase tracking-widest">Hạng thẻ</div>
                         <div className="text-primary font-black">PLATINUM</div>
                      </div>
                   </div>
                   <div className="mb-10 uppercase tracking-widest font-bold">NGUYỄN VĂN A</div>
                   <div className="flex justify-between items-end">
                      <div>
                         <div className="text-[10px] text-slate-400 uppercase">Điểm tích lũy</div>
                         <div className="text-3xl font-black text-primary">5,240 <span className="text-xs">pts</span></div>
                      </div>
                      <div className="bg-slate-400 w-12 h-8 rounded-lg opacity-20"></div>
                   </div>
                </div>
             </div>
           </div>
         </section>
      </div>
    </div>
  );
}
