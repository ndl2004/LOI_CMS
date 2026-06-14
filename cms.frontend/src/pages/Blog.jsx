import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import axiosClient from "../api/axiosClient";
import { IMAGE_BASE_URL } from "../config";

function Blog() {
    const [posts, setPosts] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);

    const pageSize = 6;

    const getImageUrl = (imageUrl) => {
        if (!imageUrl)
            return "https://via.placeholder.com/600x360?text=LOI+Cosmetics";

        if (imageUrl.startsWith("http"))
            return imageUrl;

        return `${IMAGE_BASE_URL}${imageUrl}`;
    };

    useEffect(() => {
        axiosClient
            .get("/Posts")
            .then((res) => setPosts(res.data))
            .catch((err) => console.log("Lỗi lấy bài viết:", err));
    }, []);

    const totalPages = Math.ceil(posts.length / pageSize);

    const pagedPosts = posts.slice(
        (currentPage - 1) * pageSize,
        currentPage * pageSize
    );

    return (
        <div className="container">
            <section className="blog-section">
                <div className="blog-head">
                    <div>
                        <span className="blog-subtitle">
                            Beauty Blog
                        </span>

                        <h2>Cẩm nang làm đẹp</h2>

                        <p>
                            Tổng hợp các bài viết chăm sóc da và mỹ phẩm.
                        </p>
                    </div>
                </div>

                <div className="blog-grid">
                    {pagedPosts.map((post) => (
                        <article
                            className="blog-card"
                            key={post.id}
                        >
                            <div className="blog-image">
                                <img
                                    src={getImageUrl(post.imageUrl)}
                                    alt={post.title}
                                />

                                <span className="blog-badge">
                                    {post.categoryName || "Làm đẹp"}
                                </span>
                            </div>

                            <div className="blog-content">
                                <div className="blog-date">
                                    {post.createdDate
                                        ? new Date(
                                            post.createdDate
                                        ).toLocaleDateString("vi-VN")
                                        : "LOI Cosmetics"}
                                </div>

                                <h3>{post.title}</h3>

                                <p>
                                    {post.content
                                        ? post.content
                                            .replace(/<[^>]+>/g, "")
                                            .substring(0, 120) + "..."
                                        : "Cập nhật kiến thức làm đẹp và chăm sóc da."}
                                </p>

                                <Link
                                    className="blog-read-more"
                                    to={`/blog/${post.id}`}
                                >
                                    Đọc bài viết →
                                </Link>
                            </div>
                        </article>
                    ))}
                </div>

                {totalPages > 1 && (
                    <div className="pagination">
                        <button
                            className="pagination-nav"
                            disabled={currentPage === 1}
                            onClick={() =>
                                setCurrentPage(currentPage - 1)
                            }
                        >
                            ← Trước
                        </button>

                        {[...Array(totalPages)].map((_, index) => (
                            <button
                                key={index}
                                className={
                                    currentPage === index + 1
                                        ? "active"
                                        : ""
                                }
                                onClick={() =>
                                    setCurrentPage(index + 1)
                                }
                            >
                                {index + 1}
                            </button>
                        ))}

                        <button
                            className="pagination-nav"
                            disabled={
                                currentPage === totalPages
                            }
                            onClick={() =>
                                setCurrentPage(currentPage + 1)
                            }
                        >
                            Sau →
                        </button>
                    </div>
                )}
            </section>
        </div>
    );
}

export default Blog;