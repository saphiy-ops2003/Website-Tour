import { motion } from 'motion/react';
import { Search, Heart, Star, Clock, Filter, ChevronLeft, ChevronRight } from 'lucide-react';
import TourCard from '../components/TourCard';

interface Tour {
  title: string;
  price: number;
  rating: number;
  duration: string;
  img: string;
  badge?: string;
}

export default function Tours() {
  const tours: Tour[] = [
    { title: "Du thuyền Hạ Long Sang Trọng & Khám Phá Hang Động", price: 299, rating: 4.9, duration: "3 Ngày 2 Đêm", img: "https://lh3.googleusercontent.com/aida-public/AB6AXuCJTc_bIx0R9u9bMrsmfty95mpAXFuDUqa1VYdHkrVHgd_74X7EqkUAHVcun8lVCF3TysT9-0qO_Pqo0sP2EJL8IbmueM6TXhKEpWu-_amunCq9n5wNclfHxPYZ0GhEWoKqE3MAnAI8erj7nng8-PPiUya6Y3x66iI09VCOM7AjL_RkZsYP9PGtszH726we_GH9-lUkopXFhvJwNtPtsNZ0MwrKW1yd4gWE_9GeAsP-5ALuOLoQwk1SWm-yeb0o2Kh0fYpRd89fQQ", badge: "Bán Chạy Nhất" },
    { title: "Phố Cổ Hội An: Hành Trình Khám Phá Ẩm Thực", price: 45, rating: 4.8, duration: "1 Ngày", img: "https://lh3.googleusercontent.com/aida-public/AB6AXuC0peQDZIFRKmoUdMAIjQIcujoEY4Oz_5s3-szquMDkyI0HTEWDCAyvGSV3RceLm8wWQlCCAgwb5VEsLQWMnDa19wJ64sVVFOj66FTLc7HYkgt-5huPeL7DQfPhacFvRiKachpls2USX2jqkGzKlk6pjj8tlNuIqDX4vyH1vMyUjpRqAW2pqMeAgJ3Y0frsnZkx-hlb7UUkhoR_Be3Gb-57ZBfkfKFhdU1uuhCQDOgqECpQv_PsO5yjKJj8rSISC16pFYpXdg553A", badge: "Đánh Giá Cao" },
    { title: "Trekking Sapa: Trải nghiệm Homestay Bản Làng", price: 185, rating: 4.7, duration: "4 Ngày 3 Đêm", img: "https://lh3.googleusercontent.com/aida-public/AB6AXuDsuAOAmyHthVIPgSMFE-vu1vf667YpKH2BzbR0xK1AJP7T1U358VvO7gZN3tCeo7rU_N7WKz8L4GTCT4l814TcIWhoTfpC3xjdt9Q2FSQaTHFzZ7Sv1bcquMmGBaYMqaARJjRt5Wie2JtWgaOXYJUy47jPP1MY9xO_JQahe4_HKdNHkpg2ZLJuxkhWwSbdmISO4rxBtS-MLQ2slYiopi7fuzMS2mbhQbWaGXqzpTZDPzaBgWvgqoX81fgtPEbYj4ZRBVzI6-YvCQ" },
    { title: "Lịch Sử Cố Đô Huế: Tour Riêng Có Hướng Dẫn", price: 120, rating: 4.9, duration: "2 Ngày 1 Đêm", img: "https://lh3.googleusercontent.com/aida-public/AB6AXuBjD2ZfgCHq_DJB_6CsU_F9jPuUmwEWhQXm4OtaLWjpxB2Zf1cGU_VqXiRJchq30MPhBVUv6IRn8ngBnZ_ZBB4pW_u8xy1vybNrZeSXFAdUYbYHZkig74CutqXukqEX-QehReV_lKY70ZO6Dx4gxfCqXWFIbIpgpO2W6KYoTOndR7hhjKqWOPovLDQrLcTMaevaHA6P8rZLAhqwY5KKOdrbMKlLfTrAD8ZdRnmu9soGjv2ak05DgIFY3HJdUFFEZmaClVbLCqO7eQ" },
    { title: "Hành Trình Khám Phá Đà Nẵng & Bà Nà Hills", price: 210, rating: 4.6, duration: "3 Ngày 2 Đêm", img: "https://lh3.googleusercontent.com/aida-public/AB6AXuBlszLUNX8v4IwqNybSE_U6dYKd9C242dd7t1SAV4Cp2OvO1B1IGPrqt_ReoUp3TPJTtNASkaQDOYf25JiQjkku7wWgnvPoJzwfE4FoYPnaxjtdISOvlLcZplzzwnhBdcS25xqRoD4GfVnTEcabkcQcsoVo_R1dzdJqEXozV4QVkaexJFVLduDjFXvSWE5N9cDLS5eob6WM8cYGx6LIO9TalnQakgYVjzdBnHhcoMHCi1LLGzDiyjLYgT3yjrdDEmt1BwDwM7t0hg", badge: "Bán Chạy Nhất" },
    { title: "Nhịp Sống Mekong: Chợ Nổi & Miền Sông Nước", price: 89, rating: 4.8, duration: "2 Ngày 1 Đêm", img: "https://lh3.googleusercontent.com/aida-public/AB6AXuApNHYGRKWoFthYDJGYF2a_-_0Dxf3CKErKTtW09xhnCW6uP1h9GXQQf22iYijgCZKwCGq4QPhUUDh8Gute5DO0L5Jn0h7A-phuwM6WDS3wMEpHVQOPI2v2bRXlR1nwbN-ZFfYwHN6QnZvWiUH-5X5BaDp9fmp3NhdfXLFQjknCs6jC97GqpIrEHRyt6wZPcm3cBiBwb1nX-CnNbSt0ZFnZmPPQsj0BLemTb0kieDIb635Oqlq6Y1E-5XynJheuU1_7vLGZRXX16Q" },
  ];

  return (
    <div className="bg-slate-50 min-h-screen py-12">
      <div className="max-w-7xl mx-auto px-6">
        <div className="flex flex-col md:flex-row gap-8">
          {/* Sidebar */}
          <aside className="w-full md:w-1/4 space-y-8">
            <div className="bg-white p-8 rounded-2xl border border-slate-200 shadow-sm sticky top-24">
              <h3 className="text-2xl font-bold text-primary-dark mb-8 flex items-center gap-2">
                <Filter className="w-5 h-5" /> Bộ Lọc
              </h3>
              
              <div className="space-y-10">
                <div>
                  <label className="text-[10px] font-bold text-slate-400 uppercase tracking-widest block mb-4">Khoảng Giá</label>
                  <input type="range" className="w-full h-2 bg-slate-100 rounded-lg accent-primary cursor-pointer" />
                  <div className="flex justify-between mt-3 text-sm text-slate-500 font-medium">
                    <span>$0</span>
                    <span>$2,000+</span>
                  </div>
                </div>

                <div>
                   <label className="text-[10px] font-bold text-slate-400 uppercase tracking-widest block mb-4">Thời Gian</label>
                   <div className="space-y-3">
                      {['1-3 ngày', '4-7 ngày', 'Trên 1 tuần'].map((t, i) => (
                        <label key={i} className="flex items-center gap-3 cursor-pointer group">
                           <input type="checkbox" className="rounded text-primary focus:ring-primary w-5 h-5 border-slate-300" defaultChecked={i === 1} />
                           <span className="text-slate-600 group-hover:text-primary transition-colors">{t}</span>
                        </label>
                      ))}
                   </div>
                </div>

                <div>
                   <label className="text-[10px] font-bold text-slate-400 uppercase tracking-widest block mb-4">Vùng Miền</label>
                   <div className="flex flex-wrap gap-2">
                      {['Miền Bắc', 'Miền Trung', 'Miền Nam'].map((r, i) => (
                        <button key={i} className={`px-4 py-2 rounded-full text-xs font-semibold transition-all ${i === 1 ? 'bg-primary text-white shadow-lg' : 'border border-slate-200 text-slate-600 hover:border-primary'}`}>
                          {r}
                        </button>
                      ))}
                   </div>
                </div>
              </div>

              <button className="w-full mt-12 py-3.5 bg-slate-100 text-primary-dark font-bold rounded-xl hover:bg-slate-200 transition-all">
                Xóa Tất Cả
              </button>
            </div>
          </aside>

          {/* List */}
          <div className="flex-1">
             <div className="flex justify-between items-center mb-10">
                <div>
                  <h1 className="text-4xl font-bold text-primary-dark mb-1">Khám Phá Việt Nam</h1>
                  <p className="text-slate-500 font-medium">Hiển thị 24 trải nghiệm hấp dẫn</p>
                </div>
                <div className="bg-white px-4 py-2 rounded-xl border border-slate-200 flex items-center gap-3 shadow-sm">
                   <span className="text-xs font-bold text-slate-400 uppercase">Sắp xếp:</span>
                   <select className="border-none bg-transparent text-sm font-bold text-primary focus:ring-0 outline-none">
                      <option>Phổ biến nhất</option>
                      <option>Giá thấp nhất</option>
                   </select>
                </div>
             </div>

             <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                {tours.map((tour, i) => (
                  <TourCard key={i} tour={tour} />
                ))}
             </div>

             {/* Pagination */}
             <div className="mt-16 flex justify-center items-center gap-3">
                <button className="w-10 h-10 flex items-center justify-center rounded-xl border border-slate-200 hover:bg-white transition-all"><ChevronLeft /></button>
                <button className="w-10 h-10 flex items-center justify-center rounded-xl bg-primary text-white font-bold shadow-lg">1</button>
                <button className="w-10 h-10 flex items-center justify-center rounded-xl border border-slate-200 hover:bg-white transition-all font-bold">2</button>
                <button className="w-10 h-10 flex items-center justify-center rounded-xl border border-slate-200 hover:bg-white transition-all font-bold">3</button>
                <span className="text-slate-300">...</span>
                <button className="w-10 h-10 flex items-center justify-center rounded-xl border border-slate-200 hover:bg-white transition-all font-bold">8</button>
                <button className="w-10 h-10 flex items-center justify-center rounded-xl border border-slate-200 hover:bg-white transition-all"><ChevronRight /></button>
             </div>
          </div>
        </div>
      </div>
    </div>
  );
}
