FROM centos:7 AS base

ENV container docker
RUN (cd /lib/systemd/system/sysinit.target.wants/; for i in *; do [ $i == \
systemd-tmpfiles-setup.service ] || rm -f $i; done); \
rm -f /lib/systemd/system/multi-user.target.wants/*;\
rm -f /etc/systemd/system/*.wants/*;\
rm -f /lib/systemd/system/local-fs.target.wants/*; \
rm -f /lib/systemd/system/sockets.target.wants/*udev*; \
rm -f /lib/systemd/system/sockets.target.wants/*initctl*; \
rm -f /lib/systemd/system/basic.target.wants/*;\
rm -f /lib/systemd/system/anaconda.target.wants/*;
VOLUME [ "/sys/fs/cgroup" ]
RUN yum -y install git ncurses-devel cmake make gcc-c++ boost boost-devel wget unzip nano bzip2 mysql-devel mysql-lib;
CMD ["/usr/sbin/init"]
#FROM base
#WORKDIR /home/replicator
#git clone https://github.com/tarantool/mysql-tarantool-replication.git; \
#cd mysql-tarantool-replication; \
#git submodule update --init --recursive; \
#cmake .; \
#make; \
#sed -i 's/ExecStart=\/usr\/local\/sbin\/replicatord -c \/usr\/local\/etc\/replicatord\.cfg/ExecStart=\/usr\/local\/sbin\/replicatord -c \/usr\/local\/etc\/replicatord\.yml/' replicatord.service; \
#cp replicatord /usr/local/sbin/replicatord; \
#cp replicatord.service /etc/systemd/system;
#COPY replicatord.yml /usr/local/etc/replicatord.yml
#
#CMD ["replicatord"]