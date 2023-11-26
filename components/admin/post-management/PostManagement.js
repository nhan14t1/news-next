import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Button, Card, Table } from "antd";
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import Link from "next/link";

const PostManagement = () => {
  const dataSource = [
    {
      id: '1',
      author: 'Mike',
      views: 32,
      title: '10 Downing Street',
    },
    {
      id: '2',
      author: 'John',
      views: 42,
      title: '10 Downing Street',
    },
  ];

  const columns = [
    {
      title: 'Tiêu đề',
      dataIndex: 'title',
      key: 'title',
    },
    {
      title: 'Người đăng',
      dataIndex: 'author',
      key: 'author',
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
    },
    {
      title: 'Categories',
      dataIndex: 'categories',
      key: 'categories',
    },
    {
      title: 'Tags',
      dataIndex: 'tags',
      key: 'tags',
    },
    {
      title: 'Ngày tạo',
      dataIndex: 'createdDate',
      key: 'createdDate',
    },
    {
      title: 'Lên lịch',
      dataIndex: 'scheduleDate',
      key: 'scheduleDate',
    },
    {
      title: 'Lượt xem',
      dataIndex: 'views',
      key: 'views',
    },
    {
      title: '',
      key: 'actions',
      render: (_, item) => {
        return <>
          <Button key={`btnUpdate${item.id}`} type="primary">Sửa</Button> &nbsp;
          <Button key={`btnDelete${item.id}`} type="primary" danger>Xóa</Button>
        </>
      }
    },
  ];

  return <>
    <Card>
      <h2>Quản lý bài viết</h2>
      <br />

      <div className="d-flex">
        <Link href="/admin/post-management/new-post">
          <Button type="primary" className="ms-auto">
            <><FontAwesomeIcon icon={faPlus} />&nbsp; Bài viết mới</>
          </Button>
        </Link>

      </div>

      <Table dataSource={dataSource} columns={columns} className="mt-2" />;
    </Card>
  </>;
}

export default PostManagement;